// Guardian Actors
// Copyright (c) 2023 CROQUET CORPORATION
// The Guardian game is basically a 2D game. Virtually all computations in the model are 2D.
// The flat world is placed on a Perlin noise generated surface in the view, but all interactions including
// driving and collisions are computed in 2D.

import { Constants, Actor, mix, AM_Spatial, AM_Behavioral, v3_add, v3_sub, UserManager, User, AM_Avatar, q_axisAngle, v3_normalize, v3_rotate, AM_Grid, AM_OnGrid } from "@croquet/worldcore-kernel";
import { GameModelRoot } from "@croquet/game-models";

Constants.versionBump = 0; // change this to force model to be rebuilt

const v_dist2Sqr = function (a,b) {
    const dx = a[0] - b[0];
    const dy = a[2] - b[2];
    return dx*dx+dy*dy;
};

const v_mag2Sqr = function (a) {
    return a[0]*a[0]+a[2]*a[2];
};

//------------------------------------------------------------------------------------------
//-- BaseActor -----------------------------------------------------------------------------
// This is the ground plane.
//------------------------------------------------------------------------------------------

class BaseActor extends mix(Actor).with(AM_Spatial, AM_Grid) {

    get pawn() {return "BasePawn"}
    get gamePawnType() { return "" } // don't build a connected pawn for Unity

}
BaseActor.register('BaseActor');

//------------------------------------------------------------------------------------------
// HealthCoinActor ---------------------------------------------------------------------------
// Displays the current state of health of the tower in a spinning coin
//------------------------------------------------------------------------------------------

class HealthCoinActor extends mix(Actor).with(AM_Spatial) {
    get pawn() { return "HealthCoinPawn" }
    get gamePawnType() { return "healthcoin" }

    init(...args) {
        super.init(...args);
        this.angle = 0;
        this.deltaAngle = 0.1; // radians per 100ms step
        this.spin();
    }

    spin() {
        this.angle+=this.deltaAngle;
        this.set({rotation: q_axisAngle([0,1,0], this.angle)});
        this.future(100).spin();
    }

}
HealthCoinActor.register('HealthCoinActor');

//------------------------------------------------------------------------------------------
// FireballActor ---------------------------------------------------------------------------
// Bot explosions - small one when you shoot them, big one when they suicide at the tower
//------------------------------------------------------------------------------------------

class FireballActor extends mix(Actor).with(AM_Spatial) {
    get pawn() { return "FireballPawn" }
    get gamePawnType() { return "fireball" }

    get onTarget() { return this._onTarget }

    init(...args) {
        super.init(...args);
        this.timeScale = 0.00025 + Math.random()*0.00002;
        this.future(2000).destroy();
    }

}
FireballActor.register('FireballActor');

//------------------------------------------------------------------------------------------
// BotActor --------------------------------------------------------------------------------
// The bad guys - they try to get to the tower to blow it up
//------------------------------------------------------------------------------------------
class BotActor extends mix(Actor).with(AM_Spatial, AM_OnGrid, AM_Behavioral) {
    get pawn() { return "BotPawn" }
    get gamePawnType() { return "bot" }

    get index() {return this._index || 0}

    init(options) {
        super.init(options);
        this.radius = 5;
        this.radiusSqr = this.radius*this.radius;
        this.doFlee();
        this.go([0,0,0]);
    }

    go(target) {
        // console.log(target);
        if (this.ggg) {
            this.ggg.destroy();
            this.ggg = null;
        }
        const speed = (16 + 4 * Math.random());
        this.ggg = this.behavior.start( {name: "GotoBehavior", target, speed, noise:2, radius:1} );
    }

    killMe(s=0.3, onTarget) {
        FireballActor.create({translation:this.translation, scale:[s,s,s], onTarget});
        this.publish("bots", "destroyedBot", onTarget);
        this.destroy();
    }

    resetGame() {
        if (this.ggg) {
            this.ggg.destroy();
            this.ggg = null;
        }
        this.destroy();
    }

    doFlee() {
        // blow up at the tower
        if ( v_mag2Sqr(this.translation) < 20 ) this.killMe(1, true);
        // otherwise, check if we need to move around an object
        if (!this.doomed) {
            this.future(100).doFlee();
            const blockers = this.pingAll("block");
            if (blockers.length===0) return;
            blockers.forEach(blocker => this.flee(blocker));
        }
    }

    flee(bot) {
        const from = v3_sub(this.translation, bot.translation);
        const mag2 = v_mag2Sqr(from);
        if (mag2 > this.radiusSqr) return;
        if (mag2===0) {
            const a = Math.random() * 2 * Math.PI;
            from[0] = this.radius * Math.cos(a);
            from[1] = 0;
            from[2] = this.radius* Math.sin(a);
        } else {
            let mag = Math.sqrt(mag2);
            if (bot.isAvatar) mag/=2;
            from[0] = this.radius * from[0] / mag;
            from[1] = 0;
            from[2] = this.radius * from[2] / mag;
        }
        const translation = v3_add(this.translation, from);
        this.set({translation});
    }

}
BotActor.register("BotActor");

//------------------------------------------------------------------------------------------
//--BollardActor, TowerActor ---------------------------------------------------------------
// Actors that place themselves on the grid so other actors can avoid them
//------------------------------------------------------------------------------------------

class BollardActor extends mix(Actor).with(AM_Spatial, AM_OnGrid) {
    get pawn() { return "BollardPawn" }
    get gamePawnType() { return "bollard" }

    get radius() { return this._radius }
}
BollardActor.register('BollardActor');

class TowerActor extends mix(Actor).with(AM_Spatial, AM_OnGrid) {
    get pawn() { return  "TowerPawn"}
    get gamePawnType() { return this._index >= 0 ? `tower${this._index}` : "" } // tower "-1" has no pawn; actor collisions only

    get radius() { return this._radius || 0 } // central tower isn't even assigned a radius
}
TowerActor.register('TowerActor');

//------------------------------------------------------------------------------------------
//--MissileActor ---------------------------------------------------------------------------
// Fired by the tank - they destroy the bots but bounce off of everything else
//------------------------------------------------------------------------------------------
const missileSpeed = 75;

class MissileActor extends mix(Actor).with(AM_Spatial, AM_Behavioral) {
    get pawn() { return "MissilePawn" }
    get gamePawnType() { return "missile" }

    init(options) {
        super.init(options);
        this.future(8000).destroy(); // destroy after some time
        this.lastTranslation = [0,0,0];
        this.lastBounce = null; // the thing we last bounced off
        this.tick();
    }

    resetGame() {
        this.destroy();
    }

    get colorIndex() { return this._colorIndex }

    tick() {
        this.test();
        if (!this.doomed) this.future(10).tick();
    }

    test() {
        const bot = this.parent.pingAny("bot", this.translation, 4, this);
        if (bot) {
            const d2 = v_dist2Sqr(this.translation, bot.translation);
            if (d2 < 4) { // bot radius is 2
                bot.killMe(0.3, false);
                // console.log(`bot ${bot.id} hit at distance ${Math.sqrt(d2).toFixed(2)}`);
                this.destroy();
                return;
            }
        }

        // the blockers (tagged with "block") include all avatars
        const blocker = this.parent.pingAny("block", this.translation, 4, this);
        if (blocker) {
            if (!this.lastBounce && blocker.tags.has("avatar") && blocker.colorIndex === this.colorIndex) {
                // ignore own avatar when it's the first object we've encountered
            } else if (blocker !== this.lastBounce) {
                const d2 = v_dist2Sqr(this.translation, blocker.translation);
                if (d2 < 2.5) {
                    // console.log("bounce", blocker);
                    this.lastBounce = blocker;
                    let aim = v3_sub(this.translation, blocker.translation);
                    aim[1]=0;
                    aim = v3_normalize(aim);
                    if (this.go) this.go.destroy();
                    this.go = this.behavior.start({name: "GoBehavior", aim, speed: missileSpeed, tickRate: 20});
                    this.ballisticVelocity = aim.map(val => val * missileSpeed);
                }
            }
        }

        this.lastTranslation = this.translation;
    }
}
MissileActor.register('MissileActor');

//------------------------------------------------------------------------------------------
//-- AvatarActor ---------------------------------------------------------------------------
// This is you. Most of the control code for the avatar is in the pawn in Avatar.js.
//------------------------------------------------------------------------------------------

class AvatarActor extends mix(Actor).with(AM_Spatial, AM_Avatar, AM_OnGrid) {
    get pawn() { return "AvatarPawn" }
    get gamePawnType() { return "tank" }

    init(options) {
        super.init(options);
        this.isAvatar = true;
        this.listen("shoot", this.doShoot);
        this.subscribe("all", "godMode", this.doGodMode);
    }

    get colorIndex() { return this._colorIndex }

    doGodMode(gm) {
        this.publish("all", "godModeChanged", gm);
    }

    doShoot(argFloats) {
        // view is now expected to set the launch location, given that the launcher
        // can compensate for its own velocity
        const [ x, y, z, yaw ] = argFloats;
        const aim = v3_rotate([0,0,1], q_axisAngle([0,1,0], yaw));
        const translation = [x, y, z]; // v3_add([x, y, z], v3_scale(aim, 5));
        const missile = MissileActor.create({parent: this.parent, translation, colorIndex: this.colorIndex});
        missile.go = missile.behavior.start({name: "GoBehavior", aim, speed: missileSpeed, tickRate: 20});
        missile.ballisticVelocity = aim.map(val => val * missileSpeed);
    }

    resetGame() { // don't go home at end of game
        // this.say("goHome");
    }
}
AvatarActor.register('AvatarActor');

//------------------------------------------------------------------------------------------
//-- Users ---------------------------------------------------------------------------------
// Create a new avatar when a new user joins.
//------------------------------------------------------------------------------------------

class MyUserManager extends UserManager {
    init() {
        super.init();
        this.props = new Map();
        this.propsTimeout = 60*60*1000; // 1 hour
    }

    get defaultUser() {return MyUser}

    createUser(options) {
        const { userId } = options;
        // restore saved props
        const saved = this.props.get(userId);
        if (saved) {
            options = {...options, savedProps: saved.props};
            this.props.delete(userId);
        }
        // delete old saved props
        const expired = this.now() - this.propsTimeout;
        for (const [uid, {lastSeen}] of this.props) {
            if (lastSeen < expired) {
                this.props.delete(uid);
            }
        }
        return super.createUser(options);
   }

    destroyUser(user) {
        const props = user.saveProps();
        if (props) {
            this.props.set(user.userId, {props, lastSeen: this.now()});
        }
        super.destroyUser(user);
    }
}
MyUserManager.register('MyUserManager');

class MyUser extends User {
    init(options) {
        super.init(options);
        // console.log(options);
        const base = this.wellKnownModel("ModelRoot").base;

        const placementAngle = Math.random() * Math.PI * 2;
        const placementDist = 15 + Math.random() * 30; // 15 to 45 (closest bollard is around 50 from centre)
        // choose an orientation that isn't out along the placement spoke, in case
        // we're near the tower and the camera behind us gets blocked
        const yaw = placementAngle + Math.PI + (1 - Math.random() * 2) * Math.PI/2;
        const props = options.savedProps || {
            colorIndex: options.userNumber%24,
            translation: [placementDist * Math.sin(placementAngle), 0, placementDist * Math.cos(placementAngle)],
            rotation: q_axisAngle([0,1,0], yaw),
        };

        this.avatar = AvatarActor.create({
            parent: base,
            driver: this.userId,
            tags: ["avatar", "block"],
            ...props
        });
    }

    saveProps() {
        const { color, colorIndex, translation, rotation } = this.avatar;
        return { color, colorIndex, translation, rotation };
    }

    destroy() {
        super.destroy();
        if (this.avatar) this.avatar.destroy();
    }
}
MyUser.register('MyUser');

//------------------------------------------------------------------------------------------
//-- GameStateActor ------------------------------------------------------------------------
// Manage global game state.
//------------------------------------------------------------------------------------------

class GameStateActor extends Actor {
    // get pawn() { return "GameStatePawn" } // if needed
    get gamePawnType() { return "gamestate" }

    init(options) {
        super.init(options);
        this.demoMode = false;

        this.subscribe("game", "gameStarted", this.gameStarted); // from ModelRoot.startGame
        this.subscribe("game", "undying", this.undying); // from user input
        this.subscribe("bots", "madeWave", this.madeBotWave); // from ModelRoot.makeWave
        this.subscribe("bots", "destroyedBot", this.destroyedBot); // from BotActor.killMe

        this.subscribe("stats", "update", this.updateStats); // from BotHUD (forcing stats to be published, as an alternative to just reading them)
    }

    gameStarted() {
        this.runKey = Math.random();
        this.wave = 0;
        this.totalBots = 0;
        this.health = 100;
        this.gameEnded = false;
        this.updateStats();
    }

    undying() {
        this.demoMode = !this.demoMode;
        console.log("demo mode is:", this.demoMode?"on":"off");
    }

    madeBotWave({ wave, addedBots }) {
        this.wave = wave;
        this.totalBots += addedBots;
        this.updateStats();
    }

    destroyedBot(onTarget) {
        this.totalBots--;
        if (onTarget && !this.demoMode) {
            this.health--;
            this.publish("stats", "health", this.health);
            if (this.health === 0) {
                console.log("publish the endGame");
                this.gameEnded = true;
                this.publish("game", "endGame");
            }
        }
        this.publish("stats", "bots", this.totalBots);
    }

    updateStats() {
        // legacy code for THREE version
        this.publish("stats", "wave", this.wave);
        this.publish("stats", "bots", this.totalBots);
        this.publish("stats", "health", this.health);

        if (this.gameEnded) this.publish("user", "endGame");
    }

}
GameStateActor.register('GameStateActor');

//------------------------------------------------------------------------------------------
//-- LobbyRelayActor -----------------------------------------------------------------------
//------------------------------------------------------------------------------------------

// Todo: make Elected into a mixin
class Elected extends Actor {
    init() {
        super.init();
        this.viewIds = new Set();
        this.electedViewId = "";
        this.subscribe(this.sessionId, "view-join", this.viewJoined);
        this.subscribe(this.sessionId, "view-exit", this.viewExited);
    }

    viewJoined(viewId) {
        this.viewIds.add(viewId);
        this.viewsChanged();
    }

    viewExited(viewId) {
        this.viewIds.delete(viewId);
        this.viewsChanged();
    }

    viewsChanged() {
        if (!this.viewIds.has(this.electedViewId)) {
            this.electedViewId = this.viewIds.values().next().value;
            this.viewElected(this.electedViewId);
            // console.log(this.now(), "elected", this.electedViewId);
        }
    }

    viewElected(viewId) {
        this.publish(this.sessionId, "elected-view", viewId);
    }
}
Elected.register("Elected");

class LobbyRelayActor extends Elected {
    get pawn() { return "LobbyRelayPawn" }
    get gamePawnType() { return "" } // will create a vanilla Pawn

    init() {
        super.init();
        this.beWellKnownAs("lobbyRelayActor");
        this.changeId = 0;
        this.toRelay = null;
    }

    viewsChanged() {
        super.viewsChanged();
        if (this.viewIds.size === 0) {
            this.toRelay = null;
        } else {
            this.toRelay = { changeId: ++this.changeId, views: [...this.viewIds] };
            this.say("relay-views", this.toRelay);
        }
        // console.log("relay", this.now(), "relay-views", this.toRelay);
    }

    viewElected(viewId) {
        // console.log("relay", this.now(), "relay-changed", this.electedViewId);
        this.say("relay-changed", viewId);
    }
}
LobbyRelayActor.register("LobbyRelayActor");

//------------------------------------------------------------------------------------------
//-- MyModelRoot ---------------------------------------------------------------------------
//------------------------------------------------------------------------------------------

export class MyModelRoot extends GameModelRoot {

    static modelServices() {
        return [MyUserManager, ...super.modelServices()];
    }

    init(options) {
        super.init(options);

        this.gameState = GameStateActor.create({});

        this.subscribe("game", "endGame", this.endGame); // from GameState.destroyedBot
        this.subscribe("game", "startGame", this.startGame); // from BotHUD button
        this.subscribe("game", "bots", this.demoBots); // from user input

        const bollardScale = 3; // size of the bollard
        const bollardDistance = bollardScale*3; // distance between bollards

        this.base = BaseActor.create({gridScale: bollardScale});
        this.maxBots = 1000;
        this.spawnRadius = 400;
        let v = [-10,0,0];

        // place the fins for collisions
        for (let i=0; i<3; i++) {
            const p3 = Math.PI*2/3;
            this.makeSkyscraper(v[0], 0, v[2],i*p3-Math.PI/2, -1, 1.5);
            v = v3_rotate( v, q_axisAngle([0,1,0], p3) );
        }

        let corner = 12;
        [[-corner,-corner, Math.PI/2-Math.PI/4], [-corner, corner, Math.PI/2+Math.PI/4], [corner, corner, Math.PI/2+Math.PI-Math.PI/4], [corner,-corner, Math.PI/2+Math.PI+Math.PI/4]].forEach( xy => {
            this.makeSkyscraper(bollardDistance*xy[0]+1.5, 0, bollardDistance*xy[1]+1.5,xy[2], 5, 1.5);
        });

        //place the bollards
        corner--;
        for (let x=-corner; x<=corner; x++) for (let y=-corner; y<=corner; y++) {
            if ((y<=-corner+2 || y>=corner-2) || (x<=-corner+2 || x>=corner-2) || (y<=-corner+7 && x<=-corner+7)) {
                this.makeBollard(bollardDistance*x, bollardDistance*y);
            }
        }
        const d = 290;
        // the main tower
        const tower0 = this.makeSkyscraper( 0, -1.2, 0, -0.533, 0); // no radius on central tower
        this.makeSkyscraper( 0, -1,  d, Math.PI/2, 1, 0);
        this.makeSkyscraper( 0, -1, -d, 0, 2, 0);
        this.makeSkyscraper( d, -1,  0, 0, 3, 0);
        this.makeSkyscraper(-d-10, -3,  -8, Math.PI+2.5, 4, 0);

        HealthCoinActor.create({ parent: tower0, translation: [0, 15, 0] });

        LobbyRelayActor.create();

        this.startGame();
    }

    startGame() {
        console.log("Start Game");
        this.publish("game", "gameStarted"); // alert the users to remove the start button
        this.makeWave(1, 10);
    }

    endGame() {
        console.log("End Game");
        this.service('ActorManager').actors.forEach( value => {if (value.resetGame) value.future(0).resetGame();});
    }

    demoBots( numBots ) {
        this.makeWave(0, numBots);
    }

    makeWave(wave, numBots, key = this.gameState.runKey) {
        // filter out scheduled waves from games that already finished
        if (this.gameState.gameEnded || key !== this.gameState.runKey) return;

        const { totalBots } = this.gameState;
        let actualBots = Math.min(this.maxBots, numBots);
        if ( totalBots + actualBots > this.maxBots) actualBots = this.maxBots-totalBots;

        const r = this.spawnRadius; // radius of spawn
        const a = Math.PI*2*Math.random(); // come from random direction
        for (let n = 0; n<actualBots; n++) {
            const aa = a + (0.5-Math.random())*Math.PI/4; // angle +/- Math.PI/4 around r
            const rr = r+100*Math.random();
            const x = Math.sin(aa)*rr;
            const y = Math.cos(aa)*rr;
            const index = Math.floor(20*Math.random());
            // stagger when the bots get created
            this.future(Math.floor(Math.random()*200)).makeBot(x, y, index);
        }
        if (wave>0) this.future(30000).makeWave(wave+1, Math.floor(numBots*1.2), key);

        this.publish("bots", "madeWave", { wave, addedBots: actualBots });
   }

    makeBollard(x, z) {
        BollardActor.create( { tags: ["block"], parent: this.base, obstacle: true, radius:1.5, translation: [x, 0, z]} );
    }

    makeSkyscraper(x, y, z, r, index, radius) {
        const tower = TowerActor.create( { tags: radius ? ["block"] : [], parent: this.base, index, obstacle: true, radius, translation:[x, y, z], height:y, rotation: q_axisAngle([0,1,0],r)} );
        return tower;
    }

    makeBot(x, z, index) {
        const bot = BotActor.create({parent: this.base, tags: ["block", "bot"], index, radius: 2, translation: [x, 0.5, z]});
        return bot;
    }
}
MyModelRoot.register("MyModelRoot");
