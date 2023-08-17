import { StartSession, GameViewRoot } from "@croquet/unity-bridge"; // eslint-disable-line import/no-unresolved
import { MyModelRoot } from "./Actors";

StartSession(MyModelRoot, GameViewRoot);
