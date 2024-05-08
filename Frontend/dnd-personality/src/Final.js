import { Context } from "./Context";
import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import React, { useState, useEffect } from "react";
import { useContext } from "react";
import { Button } from "@mui/material";
import { apiURL } from "./APIURL";
import { generatePath } from "react-router-dom";
import { create } from "@mui/material/styles/createTransitions";

export default function Final(props) {
  const context = useContext(Context);

  const [name, setName] = useState();
  const [age, setAge] = useState();
  const [weight, setWeight] = useState();
  const [height, setHeight] = useState();
  const [speed, setSpeed] = useState();
  const [alignment, setAlignment] = useState();
  const [hitpoints, setHitpoints] = useState();
  const [savingThrow, setSavingThrow] = useState();
  const [languages, setLanguages] = useState();
  const [armorTypes, setArmorTypes] = useState();
  const [weaponTypes, setWeaponTypes] = useState();
  const [toolBonus, setToolBonus] = useState();
  const [skills, setSkills] = useState();
  const [addititonalBonus, setAdditionalBonus] = useState();
  const [proficiencyBonus, setProficiencyBonus] = useState();
  const [darkVision, setDarkVision] = useState();
  const [additionalAbilities, setAdditionalAbilities] = useState();
  const [specialAbility, setSpecialAbility] = useState();
  const [abilities, setAbiities] = useState();
  const [weapons, setWeapons] = useState();
  const [tools, setTools] = useState();
  const [items, setItems] = useState();
  const [hitDice, setHitDice] = useState();

  const [imageUrl, setImageUrl] = useState();
  const [story, setStory] = useState();

  useEffect(() => {
    console.log("quried");
    const data = JSON.stringify({
      origin: {
        raceId: context.raceId,
        classId: context.classId,
        subraceId: context.subraceId != null ? context.subraceId : -1,
      },
      selections: context.selection,
      generateImage: context.generateImage,
      generateStory: context.generateStory,
    });
    console.log(context.selections);

    console.log(data);

    fetch(apiURL + "/characters/generate", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: data,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        console.log("story", data.story);
        console.log("image", data.imageUrl);
        setImageUrl(data.generatedImageUrl);
        setStory(data.generatedStory);
        console.log(data);
        setAge(data.age);
        setHeight(data.height);
        setWeight(data.weight);
        setSpeed(data.speed);
        setName(data.name);
        setAlignment(
          data.alignment.alignmentSide.name +
            "-" +
            data.alignment.alignmentStrength.name
        );
        setHitpoints(
          data.hitPoints.ability.name + "+" + data.hitPoints.default
        );
        setSavingThrow(data.savingThrow.name);
        setLanguages(data.languages.map((a) => a.name).join(", "));
        setArmorTypes(data.armorTypes.map((a) => a.name).join(", "));
        setWeaponTypes(data.weaponTypes.map((a) => a.name).join(", "));
        setSkills(data.skills.map((a) => a.name).join(", "));
        setTools(data.tools.map((a) => a.name).join(", "));
        setWeapons(
          data.weapons.map((a) => a.name + " - Damage: " + a.damage).join(", ")
        );
        setItems(data.startItems.map((a) => a.name).join(", "));
        setToolBonus(data.toolsBonus);
        setAdditionalBonus(data.additionalBonus);
        setProficiencyBonus(data.proficiencyBonus);
        setDarkVision(data.darkVision);
        setHitDice(data.hitDice);
        setAdditionalAbilities(data.additionalAbilities);
        setSpecialAbility(data.specialAbility);
        setAbiities(data.abilitiesTable);
      })
      .catch((error) => console.log(error));
  }, []);

  return (
    <div className="race-menu text-center">
      <div className="border-bottom border-2 border-dark">
        <div className="d-flex justify-content-center border-bottom mb-3 border-2 border-dark">
          <h2>{name}</h2>
        </div>
        <div className="d-flex justify-content-center flex-wrap">
          <div className="abilities-list border-2 rounded-2 border border-dark p-3 m-1">
            {abilities != null && (
              <div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Strength</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Strength}</p>
                </div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Dexterity</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Dexterity}</p>
                </div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Constitution</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Constitution}</p>
                </div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Intelligence</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Intelligence}</p>
                </div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Wisdom</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Wisdom}</p>
                </div>
                <div>
                  <div className="border-2 border-bottom border-dark">
                    <strong>
                      <h4>Charisma</h4>
                    </strong>
                  </div>
                  <p className="ability">{abilities.Charisma}</p>
                </div>
              </div>
            )}
          </div>
          <div className="">
            <div className="border border-2 border-dark rounded-2 p-2 m-2 mt-1 ">
              <div className="text-center border-bottom border-2 border-dark">
                <h4>Main characteristics</h4>
              </div>
              <div className="d-flex flex-wrap  justify-content-center">
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Age</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{age}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Weight</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{weight}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Height</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{height}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Speed</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{speed}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Proficiency bonus</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{proficiencyBonus}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Hit dice</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{hitDice}</p>
                    </p>
                  </div>
                </div>
                <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                  <p>
                    <strong>
                      <div className="border-2 border-bottom border-dark">
                        <h4>Alignment</h4>
                      </div>
                      <br />
                    </strong>
                    <p className="item-final">{alignment}</p>
                  </p>
                </div>
                <div></div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Hit points</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{hitpoints}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Saving throw</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{savingThrow}</p>
                    </p>
                  </div>
                </div>
              </div>
            </div>

            <div className="border-dark border border-2 rounded-2 p-2 m-2">
              <div className="text-center border-bottom border-2 border-dark">
                <h4>Inventory</h4>
              </div>
              <div className="d-flex flex-wrap justify-content-center">
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Weapons</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{weapons}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Items</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{items}</p>
                    </p>
                  </div>
                </div>
              </div>
            </div>

            <div className="border-2 border border-dark p-2 m-2 rounded-2">
              <div className="text-center border-bottom border-2 border-dark">
                <h4>Mastery</h4>
              </div>
              <div className="d-flex flex-wrap justify-content-center">
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Languages</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{languages}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Skills</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{skills}</p>
                    </p>
                  </div>
                </div>
                <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                  <p>
                    <strong>
                      <div className="border-2 border-bottom border-dark">
                        <h4>Tools</h4>
                      </div>
                      <br />
                    </strong>
                    <p className="item-final">{tools}</p>
                  </p>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Armor types</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{armorTypes}</p>
                    </p>
                  </div>
                </div>
                <div>
                  <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                    <p>
                      <strong>
                        <div className="border-2 border-bottom border-dark">
                          <h4>Weapon types</h4>
                        </div>
                        <br />
                      </strong>
                      <p className="item-final">{weaponTypes}</p>
                    </p>
                  </div>
                </div>
              </div>
            </div>

            <div className="border-2 border border-dark p-2 m-2 rounded-2">
              <div className="text-center border-bottom border-2 border-dark">
                <h4>Additional</h4>
              </div>
              <div>
                <div className="d-flex flex-wrap justify-content-center">
                  <div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                      <p>
                        <strong>
                          <div className="border-2 border-bottom border-dark">
                            <h4>Additional bonus</h4>
                          </div>
                          <br />
                        </strong>
                        <p className="item-final">{addititonalBonus}</p>
                      </p>
                    </div>
                  </div>
                  <div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                      <p>
                        <strong>
                          <div className="border-2 border-bottom border-dark">
                            <h4>Dark vision</h4>
                          </div>
                          <br />
                        </strong>
                        <p className="item-final">{darkVision}</p>
                      </p>
                    </div>
                  </div>
                  <div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                      <p>
                        <strong>
                          <div className="border-2 border-bottom border-dark">
                            <h4>Additional abilities</h4>
                          </div>
                          <br />
                        </strong>
                        <p className="item-final">{additionalAbilities}</p>
                      </p>
                    </div>
                  </div>
                  <div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                      <p>
                        <strong>
                          <div className="border-2 border-bottom border-dark">
                            <h4>Special ability</h4>
                          </div>
                          <br />
                        </strong>
                        <p className="item-final">{specialAbility}</p>
                      </p>
                    </div>
                  </div>
                  <div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                      <p>
                        <strong>
                          <div className="border-2 border-bottom border-dark">
                            <h4>Tools bonus</h4>
                          </div>
                          <br />
                        </strong>
                        <p className="item-final">{toolBonus}</p>
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {story !== null && story !== undefined && story !== "" && (
              <div className="border-2 border border-dark p-2 m-2 rounded-2">
                <div className="text-center border-bottom border-2 border-dark">
                  <h4>Story</h4>
                </div>
                <div>{story}</div>
              </div>
            )}

            {imageUrl !== null && imageUrl !== undefined && imageUrl !== "" && (
              <div className="border-2 border border-dark p-2 m-2 rounded-2">
                <div className="text-center border-bottom border-2 border-dark">
                  <h4>Image</h4>
                </div>
                <img src={imageUrl} alt="Character" />
              </div>
            )}
          </div>
        </div>
      </div>
      <div className="d-flex justify-content-center p-2">
        <Button
          onClick={() => {
            context.raceId = null;
            context.subraceId = null;
            context.classId = null;
            props.this(false);
            props.next(true);
          }}
          variant="outlined"
        >
          <b>Generate new!</b>
        </Button>
      </div>
    </div>
  );
}
