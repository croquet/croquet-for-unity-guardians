// Guardian Pawns
// Copyright (c) 2023 CROQUET CORPORATION

// In Unity we don't normally need Pawn classes, because the actual pawns are on the C# side.
//
// However, the Lobby logic is completely implemented on the JS side right now, so
// we define the LobbyRelayPawn class to relay messages to the Lobby session.
// (currently only working in the WebGL build, where the lobby launches the unity game in an iframe)

import { Pawn } from "@croquet/worldcore-kernel";

//------------------------------------------------------------------------------------------
//-- LobbyRelayPawn ----------------------------------------------------------------------------
//------------------------------------------------------------------------------------------

const MAX_USERS = 8;

class LobbyRelayPawn extends Pawn {
    constructor(model) {
        super(model);
        // this.subscribe(this.viewId,'kills', (x=> {
        //     console.log("pawn kills", x, 'v viewId=', this.viewId);
        // }))
        const inIFrame = window && window.parent !== window;
        if (!inIFrame) {
            console.warn("Disabling lobby (only works in an iframe, i.e. WebGL build)");
            return;
        }
        this.model = model;
        this.listen("relay-changed", this.relayChanged);
        console.log("relay", this.viewId, "created");
        this.relayChanged(this.model.electedViewId);
    }

    relayChanged(viewId) {
        console.log("relay", this.viewId, "relay changed to", viewId, this.viewId === viewId ? "(me)" : "(not me)");
        clearInterval(this.lobbyInterval);
        if (viewId === this.viewId) {
            this.reportToLobby();
            this.lobbyInterval = setInterval(() => this.reportToLobby(), 1000);
        }
    }

    destroy() {
        clearInterval(this.lobbyInterval);
        super.destroy();
        console.log("relay", this.viewId, "destroyed");
    }

    reportToLobby() {
        let description = `${this.model.viewIds.size} player${this.model.viewIds.size === 1 ? "" : "s"}`;
        const locations = new Map();
        let unknown = false;
        for (const viewId of this.model.viewIds) {
            const loc = CROQUETVM.views[viewId]?.loc;  // FIXME: CROQUETVM is for debugging only
            if (loc?.country) {
                let location = loc.country;
                if (loc.region) location = loc.region + ", " + location;
                if (loc.city) location = loc.city.name + " (" + location + ")";
                locations.set(location, (locations.get(location) || 0) + 1);
            } else {
                unknown = true;
            }
        }
        if (locations.size > 0) {
            let sorted = [...locations].sort((a, b) => b[1] - a[1]);
            if (sorted.length > 3) {
                sorted = sorted.slice(0, 3);
                unknown = true;
            }
            description += ` from ${sorted.map(([location]) => location).join(", ")}`;
            if (unknown) description += " and elsewhere";
        }
        const { health, demoMode } = this.wellKnownModel("modelRoot").gameState;
        description += demoMode ? " [demo]" : health ? ` [health: ${health}]` : " [waiting to play]";
        if (this.model.viewIds.size >= MAX_USERS) description += " SESSION FULL";
            // description += " [full]";
        const users = {
            count: this.model.viewIds.size,
            description,    // "3 players from USA, 2 from Canada, 1 from UK"
            color: demoMode ? "blue" : health>66 ? "green" : health>33 ? "yellow" : health>0 ? "red" : "black",
        };

        window.parent.postMessage({type: "croquet-lobby", name: this.session.name, users}, "*");
        // console.log("relay", this.viewId, "sending croquet-lobby", this.session.name, users);
    }

}
LobbyRelayPawn.register("LobbyRelayPawn");
