import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import React, {
  useState,
  useContext,
  useEffect,
  useSyncExternalStore,
} from "react";
import StepButton from "./StepButton";
import { apiURL } from "./APIURL";
import { Context } from "./Context";
import { Autocomplete, Select, TextField } from "@mui/material";
import CheckBoxOutlineBlankIcon from "@mui/icons-material/CheckBoxOutlineBlank";
import CheckBoxIcon from "@mui/icons-material/CheckBox";
import Checkbox from "@mui/material/Checkbox";
import {
  FormControl,
  FormControlLabel,
  InputLabel,
  MenuItem,
  InputAdornment,
  Card,
} from "@mui/material";

let abilitiesDefault = [
  { id: 0, name: "Strength" },
  { id: 1, name: "Dexterity" },
  { id: 2, name: "Constitution" },
  { id: 3, name: "Intelligence" },
  { id: 4, name: "Wisdom" },
  { id: 5, name: "Charisma" },
];
const emptyAbility = {
  id: null,
  name: "",
};

export default function CharacterDetails(props) {
  const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
  const checkedIcon = <CheckBoxIcon fontSize="small" />;

  const context = useContext(Context);

  const [selectors, setSelectors] = useState();

  const [names, setNames] = useState([]);
  const [name, setName] = useState();

  const [skill, setSkill] = useState([]);
  const [skills, setSkills] = useState([]);
  const [skillsAmount, setSkillsAmount] = useState(0);

  const [language, setLanguage] = useState([]);
  const [languages, setLanguages] = useState([]);
  const [languagesAmount, setLanguagesAmount] = useState(0);

  const [weapon, setWeapon] = useState([]);
  const [weapons, setWeapons] = useState([]);

  const [alignmentSide, setAlignmentSide] = useState();
  const [alignmentStrength, setAlignmentStrength] = useState();

  const [weight, setWeight] = useState();
  const [age, setAge] = useState();
  const [height, setHeight] = useState();

  const [abilities, setAbilities] = useState([]);
  const [selectedAbilities, setSelectedAbilities] = useState(Array(6));
  const [isValidationError, setIsValidationError] = useState(true);

  const [generateImage, setGenerateImage] = useState(false);
  const [generateStory, setGenerateStory] = useState(false);

  const validateInput = () => {
    for (let i = 0; i < weapons.length; i++) {
      if (weapons[i].amount != weapon[i].weapons.length) {
        setIsValidationError(true);
        return;
      }
    }

    for (let i = 0; i < selectedAbilities.length; i++) {
      if (selectedAbilities[i] == null || selectedAbilities[i].name == null) {
        setIsValidationError(true);
        return;
      }
    }

    if (
      isNaN(age) ||
      isNaN(height) ||
      isNaN(weight) ||
      name == null ||
      skill.length < skillsAmount ||
      language.length < languagesAmount ||
      alignmentSide == null ||
      alignmentStrength == null
    ) {
      setIsValidationError(true);
    } else {
      setIsValidationError(false);
    }
  };

  const createSelections = () => {
    const data = {
      specifiedItems: [
        {
          id: selectors.find((item) => item.name === "Alignment").id,
          selection: {
            alignmentSideId: alignmentSide,
            alignmentStrengthId: alignmentStrength,
          },
        },
        {
          id: selectors.find((item) => item.name === "Weight").id,
          selection: weight,
        },
        {
          id: selectors.find((item) => item.name === "Height").id,
          selection: height,
        },
        {
          id: selectors.find((item) => item.name === "Age").id,
          selection: age,
        },
        {
          id: selectors.find((item) => item.name === "Ability").id,
          selection: selectedAbilities.map((a) => a.id),
        },
      ],
      listSelectedItems: [
        {
          id: selectors.find((item) => item.name === "Language").id,
          selections: language.map((a) => a.id),
        },
        {
          id: selectors.find((item) => item.name === "Skill").id,
          selections: skill.map((a) => a.id),
        },
        {
          id: selectors.find((item) => item.name === "Name").id,
          selections: [name.id],
        },
        ...weapons.map((weaponObj) => ({
          id: weaponObj.id,
          selections: weapon
            .find((item) => item.id === weaponObj.id)
            .weapons.map((a) => a.id),
        })),
      ],
    };

    context.selection = data;
    context.generateImage = generateImage;
    context.generateStory = generateStory;
  };

  const handleAgeChange = (event) => {
    setAge(event.target.value);
    validateInput();
  };

  const handleHeightChange = (event) => {
    setHeight(event.target.value);
    validateInput();
  };

  const handleWeightChange = (event) => {
    setWeight(event.target.value);
    validateInput();
  };

  const handleAlignmentSideChange = (event) => {
    setAlignmentSide(event.target.value);
    validateInput();
  };

  const handleAlignmentStrengthChange = (event) => {
    setAlignmentStrength(event.target.value);
    validateInput();
  };

  useEffect(() => {
    const data = JSON.stringify({
      raceId: context.raceId,
      classId: context.classId,
      subraceId: context.subraceId != null ? context.subraceId : -1,
    });

    fetch(apiURL + "/characters/constuctors/generate", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: data,
    })
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        setSelectors(data);

        setNames(data.find((item) => item.name === "Name").items);

        const skillsObj = data.find((item) => item.name === "Skill");
        setSkills(skillsObj.items);
        setSkillsAmount(skillsObj.amount);

        const languagesObj = data.find((item) => item.name === "Language");
        setLanguages(languagesObj.items);
        setLanguagesAmount(languagesObj.amount);

        const weaponObj = data.filter((item) => item.name === "WeaponSelector");
        setWeapons(weaponObj);
        weaponObj.forEach((weaponObj) => {
          setWeapon([...weapon, { id: weaponObj.id, weapons: [] }]);
        });

        const firstAbilityId = data.find((item) => item.name === "Ability")
          .item[0];
        abilitiesDefault = abilitiesDefault.sort((a, b) => {
          if (a.id === firstAbilityId) {
            return -1;
          } else if (b.id === firstAbilityId) {
            return 1;
          } else {
            return 0;
          }
        });

        setAbilities(abilitiesDefault);
        let abilitiesTemp = selectedAbilities;
        abilitiesTemp[0] = abilitiesDefault[0];

        for (let i = 1; i < selectedAbilities.length; i++) {
          abilitiesTemp[i] = emptyAbility;
        }

        setSelectedAbilities(abilitiesTemp);
      })
      .catch((error) => console.log(error));
  }, []);

  return (
    <div className="race-menu">
      <div className="d-flex justify-content-between">
        <StepButton
          text="Back"
          onClick={() => {
            props.this(false);
            props.previous(true);
          }}
        />
        <h4>Additional details</h4>
        <StepButton
          disabled={isValidationError}
          text="Next"
          onClick={() => {
            createSelections();
            props.this(false);
            props.next(true);
          }}
        />
      </div>
      <div className="mt-3">
        <div className="row border-bottom border-top border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 me-3 d-flex justify-content-center">
              <h5>Character's name</h5>
            </div>
            <div className="mb-3 ms-3 mt-3 d-flex justify-content-center">
              <Autocomplete
                sx={{ width: 200 }}
                size="small"
                id="combo-box-demo"
                getOptionLabel={(option) => option.raceName}
                options={names}
                onChange={(event, newValue) => {
                  setName(newValue);
                  validateInput();
                }}
                value={name}
                renderInput={(params) => (
                  <TextField size="small" {...params} label="Name" />
                )}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center flex-column">
              <div className="">
                <h5>Character's skills</h5>
              </div>
              <div className="">
                <h6>Select {skillsAmount} items from list</h6>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center">
              <Autocomplete
                multiple
                sx={{ width: 200 }}
                size="small"
                id="combo-box-demo"
                getOptionLabel={(option) => option.name}
                options={skills}
                onChange={(event, newValue) => {
                  setSkill(newValue);
                  validateInput();
                }}
                renderOption={(props, option, { selected }) => (
                  <li {...props}>
                    <Checkbox
                      icon={icon}
                      checkedIcon={checkedIcon}
                      style={{ marginRight: 8 }}
                      checked={selected}
                    />
                    {option.name}
                  </li>
                )}
                value={skill}
                getOptionDisabled={(option) => {
                  if (skill.length == skillsAmount) {
                    return true;
                  }
                }}
                renderInput={(params) => (
                  <TextField size="small" {...params} label="Skills" />
                )}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center flex-column">
              <div className="">
                <h5>Character's additional languages</h5>
              </div>
              <div className="">
                <h6>Select {languagesAmount} items from list</h6>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center">
              <Autocomplete
                multiple
                sx={{ width: 200 }}
                size="small"
                id="combo-box-demo"
                getOptionLabel={(option) => option.name}
                options={languages}
                onChange={(event, newValue) => {
                  setLanguage(newValue);
                  validateInput();
                }}
                renderOption={(props, option, { selected }) => (
                  <li {...props}>
                    <Checkbox
                      icon={icon}
                      checkedIcon={checkedIcon}
                      style={{ marginRight: 8 }}
                      checked={selected}
                    />
                    {option.name}
                  </li>
                )}
                value={language}
                getOptionDisabled={(option) => {
                  if (language.length == languagesAmount) {
                    return true;
                  }
                }}
                renderInput={(params) => (
                  <TextField size="small" {...params} label="Languages" />
                )}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          {weapons.map((weaponObj) => {
            return (
              <div className="d-flex justify-content-center">
                <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center flex-column">
                  <div className="">
                    <h5>Character's weapons</h5>
                  </div>
                  <div className="">
                    <h6>Select {weaponObj.amount} items from list</h6>
                  </div>
                </div>
                <div className="mb-3 mt-4 ms-3 d-flex justify-content-center">
                  <Autocomplete
                    multiple
                    sx={{ width: 200 }}
                    size="small"
                    id="combo-box-demo"
                    getOptionLabel={(option) => option.name}
                    options={weaponObj.items}
                    onChange={(event, newValue) => {
                      const tempWeapon = [...weapon];
                      tempWeapon.find(
                        (item) => (item.id = weaponObj.id)
                      ).weapons = newValue;
                      setWeapon(tempWeapon);
                      validateInput();
                    }}
                    value={
                      weapon.find((item) => item.id == weaponObj.id).weapons
                    }
                    renderOption={(props, option, { selected }) => (
                      <li {...props}>
                        <Checkbox
                          icon={icon}
                          checkedIcon={checkedIcon}
                          style={{ marginRight: 8 }}
                          checked={selected}
                        />
                        {option.name}
                      </li>
                    )}
                    getOptionDisabled={(option) => {
                      console.log(weaponObj);
                      if (
                        weapon.find((item) => item.id == weaponObj.id).weapons
                          .length == weaponObj.amount
                      ) {
                        return true;
                      }
                    }}
                    renderInput={(params) => (
                      <TextField size="small" {...params} label="Weapons" />
                    )}
                  />
                </div>
              </div>
            );
          })}
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center">
              <div className="">
                <h5>Character's alignment</h5>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center flex-column align-items-center">
              <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                <InputLabel>Alignment Side</InputLabel>
                <Select
                  value={alignmentSide}
                  label="Alignment Side"
                  onChange={handleAlignmentSideChange}
                >
                  <MenuItem value={0}>Any</MenuItem>
                  <MenuItem value={1}>Good</MenuItem>
                  <MenuItem value={2}>Bad</MenuItem>
                  <MenuItem value={3}>Neutral</MenuItem>
                </Select>
              </FormControl>
              <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                <InputLabel>Alignment Strength</InputLabel>
                <Select
                  value={alignmentStrength}
                  label="Alignment Strength"
                  onChange={handleAlignmentStrengthChange}
                >
                  <MenuItem value={0}>Lawful</MenuItem>
                  <MenuItem value={1}>Neutal</MenuItem>
                  <MenuItem value={2}>Chaotic</MenuItem>
                </Select>
              </FormControl>
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center">
              <div className="">
                <h5>Character's weight</h5>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center align-items-center">
              <TextField
                size="small"
                margin="normal"
                label="Weight"
                variant="outlined"
                value={weight}
                onChange={handleWeightChange}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">kg</InputAdornment>
                  ),
                }}
                error={isNaN(weight) && weight != null}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center">
              <div className="">
                <h5>Character's height</h5>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center align-items-center">
              <TextField
                size="small"
                margin="normal"
                label="Height"
                variant="outlined"
                value={height}
                onChange={handleHeightChange}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">cm</InputAdornment>
                  ),
                }}
                error={isNaN(height) && height != null}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center">
              <div className="">
                <h5>Character's age</h5>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center align-items-center">
              <TextField
                size="small"
                margin="normal"
                label="Age"
                variant="outlined"
                value={age}
                onChange={handleAgeChange}
                InputProps={{
                  endAdornment: (
                    <InputAdornment position="end">years</InputAdornment>
                  ),
                }}
                error={isNaN(age) && age != null}
              />
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center flex-column">
              <div className="">
                <h5>Character's abilities</h5>
              </div>
              <div className="text-center">
                <h6>
                  Order abilities from the most important for you to less
                  important
                </h6>
                <h6>
                  Main ability is defined by your class and can not be changed{" "}
                </h6>
              </div>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center align-items-center flex-column">
              {selectedAbilities.map((ability) => {
                return (
                  <div>
                    <Autocomplete
                      sx={{ width: 200 }}
                      disabled={ability.id === selectedAbilities[0].id}
                      size="small"
                      id="combo-box-demo"
                      margin="normal"
                      getOptionLabel={(option) => option.name}
                      options={abilities}
                      value={ability}
                      onChange={(event, newValue) => {
                        let tempAbilities = [...selectedAbilities];
                        for (var i = 0; i < tempAbilities.length; i++) {
                          if (tempAbilities[i].id == ability.id) {
                            tempAbilities[i] = newValue;
                            break;
                          }
                        }
                        setSelectedAbilities(tempAbilities);
                        validateInput();
                      }}
                      getOptionDisabled={(option) => {
                        if (selectedAbilities.includes(option)) {
                          return true;
                        } else {
                          return false;
                        }
                      }}
                      renderInput={(params) => (
                        <TextField
                          margin="dense"
                          size="small"
                          {...params}
                          label="Ability"
                        />
                      )}
                    />
                  </div>
                );
              })}
            </div>
          </div>
        </div>
        <div className="row border-bottom border-2 border-dark">
          <div className="d-flex justify-content-center">
            <div className="mt-4 mb-3 me-3 d-flex align-items-center justify-content-center flex-column">
              <h5>Additional</h5>
            </div>
            <div className="mb-3 mt-4 ms-3 d-flex justify-content-center align-items-center flex-column">
              <FormControlLabel
                control={
                  <Checkbox
                    checked={generateImage}
                    onChange={() => setGenerateImage(!generateImage)}
                  />
                }
                label="Generate image for character"
              />
              <FormControlLabel
                control={
                  <Checkbox
                    checked={generateStory}
                    onChange={() => setGenerateStory(!generateStory)}
                  />
                }
                label="Generate story for character"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
