import { Context } from "./Context";
import StartUpPage from "./StartUpPage";
import RaceMenu from "./RaceMenu";
import SubRaceMenu from "./SubRaceMenu";
import ClassMenu from "./ClassMenu";
import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import React, { useState } from "react";
import { useContext } from "react";
import CharacterDetails from "./CharacterDetails";
import Final from "./Final";

export default function Main() {
  const context = useContext(Context);

  const [isStartUp, setStartUp] = useState(true);
  const [isRaceMenu, setRaceMenu] = useState(false);
  const [isSubRaceMenu, setSubRaceMenu] = useState(false);
  const [isClassMenu, setClassMenu] = useState(false);
  const [isCharacterDetails, setCharacterDetails] = useState(false);
  const [isFinal, setFinal] = useState(false);

  return (
    <div className="Main">
      {isStartUp ? <StartUpPage this={setStartUp} next={setRaceMenu} /> : null}
      {isRaceMenu ? (
        <RaceMenu
          previous={setStartUp}
          this={setRaceMenu}
          next={setSubRaceMenu}
        />
      ) : null}
      {isSubRaceMenu ? (
        <SubRaceMenu
          previous={setRaceMenu}
          this={setSubRaceMenu}
          next={setClassMenu}
        />
      ) : null}

      {isClassMenu ? (
        <ClassMenu
          previous={[setSubRaceMenu, setRaceMenu]}
          this={setClassMenu}
          next={setCharacterDetails}
        />
      ) : null}
      {isCharacterDetails ? (
        <CharacterDetails
          previous={setClassMenu}
          this={setCharacterDetails}
          next={setFinal}
        />
      ) : null}
      {isFinal ? <Final this={setFinal} next={setStartUp} /> : null}
    </div>
  );
}
