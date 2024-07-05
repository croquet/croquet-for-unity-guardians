import { StartSession, GameViewRoot } from "@croquet/unity-bridge";
import { MyModelRoot } from "./Actors";
import "./Pawns";

StartSession(MyModelRoot, GameViewRoot);
