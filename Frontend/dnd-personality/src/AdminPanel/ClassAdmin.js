import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";
import TextField from "@mui/material/TextField";
import { FormControl, Select } from "@mui/material";
import AdminResult from "./ResultAdmin";
import { DataGrid } from '@mui/x-data-grid';
import { useEffect } from "react";
import {MenuItem} from "@mui/material";
import React, { useState } from "react";
import Button  from "@mui/material/Button";
import InputLabel from "@mui/material/InputLabel";
import Autocomplete from '@mui/material/Autocomplete';

import {apiURL} from "../APIURL";
import { useAuth0 } from "@auth0/auth0-react";

let weaponSelectorNextId = 1;
const defaultArmorTypes = [
    {name: 'LightArmor', armorTypeId:0},
    {name: 'HeavyArmor', armorTypeId:1},
    {name: 'MediumArmor', armorTypeId:2},
    {name: 'Shield', armorTypeId:3}
];

const defaultWeaponTypes = [
    {name: 'SimpleMelee', weaponTypeId:0},
    {name: 'SimpleRanged', weaponTypeId:1},
    {name: 'MartialMelee', weaponTypeId:2},
    {name: 'MartialRanged', weaponTypeId:3}
];

const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Name', width: 150 },
    {field: 'description', headerName: 'Description', width: 250 },
    {field: 'proficiencyBonus', headerName: 'Proficiency Bonus', width: 150 },
    {field: 'tools', headerName: 'Tools', width: 250, valueGetter: (params) => {
        let names = params.row.tools.map(a => a.name);
        return names;
    }},
    {field: 'skills', headerName: 'Skills', width: 250, valueGetter: (params) => {
        let names = params.row.skills.map(a => a.name);
        return names;
    }},
    {field: 'primaryAbility', headerName: 'Primary Ability', width: 150, valueGetter: (params) => {
        return params.row.primaryAbility.name;
    }},
    {field: 'savingThrow', headerName: 'Saving Throw', width: 150, valueGetter: (params) => {
        return params.row.primaryAbility.name;
    }},
    {field: 'hitDice', headerName: 'Hit Dice', width: 150 },
    {field: 'hitPoints', headerName: 'Hit Points', width: 150, valueGetter: (params) => {
        return params.row.hitPoints.ability.name+' + '+ params.row.hitPoints.default;
    } },
    {field: 'armorTypes', headerName: 'Armor Types', width: 150, valueGetter: (params) => {
        let types =params.row.armorTypes.map(a => a.name) ;
        return types;
    }},
    {field: 'weaponTypes', headerName: 'Weapon Types', width: 150, valueGetter: (params) => {
        let types =params.row.weaponTypes.map(a => a.name) ;
        return types;
    }},
    {field: 'selectedSkills', headerName: 'Selected Skills', width: 150 },
    {field: 'additionalAbilities', headerName: 'Additional Abilities', width: 150 },
    {field: 'weaponSelectors', headerName: 'Weapon Selectors', width: 150, valueGetter: (params) => {
        let items = [];
        params.row.weaponSelectors.forEach((item) => {
                items = [...items, (item.weapons.map(a => a.name).join(' or '))];
        });
        return items.join(', ');
    }},
    {field: 'startItems', headerName: 'Start Items', width: 150, valueGetter: (params) => {
        let types =params.row.startItems.map(a => a.name) ;
        return types;
    }}
];

export default function ClassAdmin(){
    const [tools, setTools] = useState([]);
    const [skills, setSkills] = useState([]);
    const [items, setItems] = useState([]);
    const [weapons, setWeapons] = useState([]);

    const [classes, setClasses] = useState([]);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [primaryAbility, setPrimaryAbility] = useState(0);
    const [savingThrow, setSavingThrow] = useState(0);
    const [proficiencyBonus, setProficiencyBonus] = useState(0);
    const [hitDice, setHitDice] = useState('');
    const [weaponTypes, setWeaponTypes] = useState([]);
    const [armorTypes, setArmorTypes] = useState([]);
    const [selectedTools, setSelectedTools] = useState([]);
    const [selectedItems, setSelectedItems] = useState([]);
    const [skillsToSelect, setSkillsToSelect] = useState([]);

    const [selectedWeapons, setSelectedWeapons] = useState([{id:0, weapon1:null, weapon2: null}]);

    const [skillsAmount, setSkillsAmount] = useState(0);
    const [additionalAbilities, setAdditionalAbilities] = useState('');
    const [hitPointValue, setHitPointValue] = useState(0);
    const [hitPointAbility, setHitPointAbility] = useState(0);

    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);

    
    const [image, setImage] = useState();
    const [imageFile, setImageFile] = useState();
    const [path, setPath] = useState('');

    const [token, setToken] = useState(null);
    const {getAccessTokenSilently} = useAuth0();

    useEffect(() => {
        (async () => {
          const accessToken = await getAccessTokenSilently();
          setToken(accessToken);
        })();
      }, []);


    function generatePath(){
        return name+Date.now();
    }

    const handleImageChange = (event) => {
        setImageFile(event.target.files[0]);
        setImage(URL.createObjectURL(event.target.files[0]));
    } 

    const handleHitPointAbilityChange = (event) => {
        setHitPointAbility(event.target.value);
    }

    const handleHitPointValueChange = (event) => {
        setHitPointValue(event.target.value);
    }

    const handleHitDiceChange = (event) => {
        setHitDice(event.target.value);
    }

    const handleSkillsAmountChange = (event) => {
        setSkillsAmount(event.target.value);
    }

    const handleAdditionalAbilitiesChange = (event) => {
        setAdditionalAbilities(event.target.value);
    }

    const handleNameChange = (event) => {
        setName(event.target.value);
    }

    const handlePrimaryAbilityChange = (event) => {
        setPrimaryAbility(event.target.value);
    }

    const handleSavingThrowChange = (event) => {
        setSavingThrow(event.target.value);
    }

    const handleDescriptionChange = (event) => {
        setDescription(event.target.value);
    }

    const handleProficiencyBonusChange = (event) => {
        setProficiencyBonus(event.target.value);
    }

    const resetInputs = (async) => {
        setImage();
        setImageFile();
        setName('');
        setDescription('');
        setSavingThrow(0);
        setPrimaryAbility(0);
        setWeaponTypes([]);
        setArmorTypes([]);
        setHitDice('');
        setSkillsToSelect([]);
        setSelectedTools([]);
        setSelectedItems([]);
        setHitPointAbility(0);
        setHitPointValue(0);
        setSelectedWeapons([{id:0, weapon1: null, weapon2: null}]);
    }

    
    const removeOnClick = async() => {
        if(selectionModel.length > 0){
            selectionModel.forEach(classId => {
                fetch(apiURL + '/images/' + classes.filter(function(classCharacter){
                    return classCharacter.id == classId
                }).imagePath +  ' /delete',{
                    method: 'DELETE',
                    headers: { 'Content-Type': 'application/json',
                    'Authorization': 'Bearer '+ token}
                });
                fetch(apiURL + '/classes/delete/' +  classId,{
                    method: 'DELETE',
                    headers: {
                        'Authorization': 'Bearer '+ token
                    }
                })
                .then((response) => {
                    setSusccesfull(response.status == 200);
                    setEditing(false);
                    setResultShown(true);
                    loadData();
                })
                .catch((error) => setError(error));  
            });
                
                   
        }      
    }

    const sendOnClick = async() => { 
        let generated = generatePath();
 
        if(isEditing){
            let data = JSON.stringify({
                id: parseInt(selectionModel),
                name: name,
                description: description,
                proficiencyBonus: proficiencyBonus,
                primaryAbilityId: primaryAbility,
                savingThrowId: savingThrow,
                hitDice: hitDice,
                hitPoints: {
                    default: hitPointValue,
                    abilityId: hitPointAbility
                },
                armorTypes: armorTypes.map(a => a.armorTypeId),
                weaponTypes: weaponTypes.map(a => a.weaponTypeId),
                tools: selectedTools,
                skills: skillsToSelect,
                selectedSkills: skillsAmount,
                additionalAbilities: additionalAbilities,
                 weaponSelectors: selectedWeapons.map(({weapon1, weapon2}) =>({
                    weapons: [
                        weapon1,
                        weapon2
                    ]
                })),
                startItems: selectedItems,
            });
            await fetch(apiURL + '/classes/update',{
                method: 'PUT',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: data
            })
            .then((response) => {
                setSusccesfull(response.status == 200);
                setEditing(false);
                setResultShown(true);
    
                loadData();
    
            })
            .catch((error) => {
                setError(error);
                setEditing(false);
            });

            let formdata = new FormData();
            formdata.append('file', imageFile);

          
            await fetch(apiURL + '/images/' + path +'/update/', {
                method: 'PUT',
                headers: {'Authorization': 'Bearer '+ token},
                body: formdata,
            })
            .then((responseImage) => {
                setSusccesfull(responseImage.status == 200);
                setResultShown(true);
                loadData();
            })
            .catch((error) => setError(error));
        }
        else{
            let data = JSON.stringify({
                name: name,
                description: description,
                proficiencyBonus: proficiencyBonus,
                primaryAbilityId: primaryAbility,
                savingThrowId: savingThrow,
                hitDice: hitDice,
                hitPoints: {
                    default: hitPointValue,
                    abilityId: hitPointAbility
                },
                armorTypes: armorTypes.map(a => a.armorTypeId),
                weaponTypes: weaponTypes.map(a => a.weaponTypeId),
                tools: selectedTools,
                skills: skillsToSelect,
                selectedSkills: skillsAmount,
                additionalAbilities: additionalAbilities,
                 weaponSelectors: selectedWeapons.map(({weapon1, weapon2}) =>({
                    weapons: [
                        weapon1,
                        weapon2
                    ]
                })),
                startItems: selectedItems,
                imagePath: generated
            });
            await fetch(apiURL + '/classes/new',{
                method: 'POST',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: data
            })
            .then((response) => {
                setSusccesfull(response.status == 201);
                setResultShown(true);
    
                loadData();
    
            })
            .catch((error) => {
                setError(error);
            });

            let formdata = new FormData();
            formdata.append('file', imageFile);
          
            await fetch(apiURL + '/images/' + generated +'/new/', {
                method: 'POST',
                headers: {'Authorization': 'Bearer '+ token},
                body: formdata,
            })
            .then((responseImage) => {
                setSusccesfull(responseImage.status == 201);
                setResultShown(true);
                loadData();
            })
            .catch((error) => setError(error));
        }
    };

    useEffect(() => {
        fetch(apiURL +'/skills/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setSkills(data);
        })
        .catch((error) => console.log(error));

        fetch(apiURL +'/items/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setItems(data);
        })
        .catch((error) => console.log(error));

        fetch(apiURL +'/tools/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setTools(data);
        })
        .catch((error) => console.log(error));

        fetch(apiURL +'/weapons/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setWeapons(data);
        })
        .catch((error) => console.log(error));
        
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = classes.filter(function(x) {return x.id == selectionModel});
            setPath(filtered[0].imagePath);
            setName(filtered[0].name);
            setDescription(filtered[0].description);
            setSavingThrow(filtered[0].savingThrow.abilityId);
            setPrimaryAbility(filtered[0].primaryAbility.abilityId);
            setHitDice(filtered[0].hitDice);

            setSkillsToSelect(filtered[0].skills.map((skillObj) =>
            {return skills.find(option => option.id === skillObj.id)}));

            setWeaponTypes(filtered[0].weaponTypes.map((weaponTypeObj) => 
                {return defaultWeaponTypes.find(option => option.weaponTypeId === weaponTypeObj.weaponTypeId)}));

            setArmorTypes(filtered[0].armorTypes.map((armorTypeObj) =>
                {return defaultArmorTypes.find(option => option.armorTypeId === armorTypeObj.armorTypeId)}));

            setSelectedTools(filtered[0].tools.map((toolObj) =>
            {return tools.find(option => option.id === toolObj.id)}));

            setSelectedItems(filtered[0].startItems.map((itemObj) =>
            {return items.find(option => option.id === itemObj.id)}));

            setSkillsAmount(filtered[0].selectedSkills);
            setHitPointAbility(filtered[0].hitPoints.ability.abilityId);
            setHitPointValue(filtered[0].hitPoints.default);
            setAdditionalAbilities(filtered[0].additionalAbilities);
            setProficiencyBonus(filtered[0].proficiencyBonus);
            let mappedWeaponSelectors = filtered[0].weaponSelectors.map(selector => ({
                id: weaponSelectorNextId++,
                weapon1: weapons.find(option => option.id === selector.weapons[0].id),
                weapon2: weapons.find(option => option.id === selector.weapons[1].id)
            }));
            setSelectedWeapons(mappedWeaponSelectors);
            setEditing(true);

            fetch(apiURL +'/images/' + path + '/get', {
                method: 'GET'
                })
                .then((response) => {
                    return response.blob();
                })
                .then((data) => {
                    setImage(URL.createObjectURL(data));
                    setImageFile(URL.createObjectURL(data));
                })
                .catch((error) => console.log(error));
        }
    }

    const loadData = async() => {
        await fetch(apiURL +'/classes/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setClasses(data);
        })
        .catch((error) => console.log(error));
    }

    return(
        <div className="accordeon-content-inside row container">
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small"  label="Name" variant="outlined"
                    margin="normal" onChange={handleNameChange} value={name}/>
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Proficiency Bonus" 
                    variant="outlined" 
                    onChange={handleProficiencyBonusChange} value={proficiencyBonus}/>   
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Hit Dice" 
                    variant="outlined" onChange={handleHitDiceChange} value={hitDice}/>       
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Selected Skills" 
                    variant="outlined" onChange={handleSkillsAmountChange} value={skillsAmount}/>                      
            </div>
            <div className="col-4">
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                        <InputLabel>Saving Throw</InputLabel>
                        <Select
                            onClick={() => {setResultShown(false);}}
                            value={savingThrow}
                            label="Armor Type"
                            
                            onChange={handleSavingThrowChange}
                        >
                            <MenuItem value={0}>Strength</MenuItem>
                            <MenuItem value={1}>Dexterity</MenuItem>
                            <MenuItem value={2}>Constitution</MenuItem>
                            <MenuItem value={3}>Intelligence</MenuItem>
                            <MenuItem value={4}>Wisdom</MenuItem>
                            <MenuItem value={5}>Charisma</MenuItem>

                        </Select>
                    </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                        <InputLabel>Primary Ability</InputLabel>
                        <Select
                            onClick={() => {setResultShown(false);}}
                            value={primaryAbility}
                            label="Armor Type"
                            
                            onChange={handlePrimaryAbilityChange}
                        >
                            <MenuItem value={0}>Strength</MenuItem>
                            <MenuItem value={1}>Dexterity</MenuItem>
                            <MenuItem value={2}>Constitution</MenuItem>
                            <MenuItem value={3}>Intelligence</MenuItem>
                            <MenuItem value={4}>Wisdom</MenuItem>
                            <MenuItem value={5}>Charisma</MenuItem>

                        </Select>
                    </FormControl>

                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setWeaponTypes(newValue);
                    }}
                    value={weaponTypes}
                    options={defaultWeaponTypes}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Weapon Types"

                    />
                    )}
                />
                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setArmorTypes(newValue);
                    }}
                    value={armorTypes}
                    options={defaultArmorTypes}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Armor Types"
                    />
                    )}
                />
            </div>
            <div className="row">
                <div className="col-7">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Description" multiline 
                    maxlines={4} variant="outlined" fullWidth
                    onChange={handleDescriptionChange} value={description}/>  
                </div>
            </div>
            <div className="row">
                <div className="col-4">
                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setSelectedTools(newValue);
                    }}
                    value={selectedTools}
                    options={tools}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Tools"
                    />
                    )}
                /> 
                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setSelectedItems(newValue);
                    }}
                    value={selectedItems}
                    options={items}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Items"
                    />
                    )}
                /> 
                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setSkillsToSelect(newValue);
                    }}
                    value={skillsToSelect}
                    options={skills}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Skills"
                    />
                    )}
                /> 
                </div>
                <div className="col-4">
                <form method="post" action="post" enctype="multipart/form-data">

                <Button sx={{ m: 1}} variant="outline" component="label" 
                    >
                    Upload File
                    <input
                    onChange={handleImageChange}
                    type="file"
                    hidden
                    />
                    </Button>
                </form>
                </div>
            </div>
            <div className="row">
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Hit Point Value" 
                        variant="outlined" value={hitPointValue} onChange={handleHitPointValueChange}
                        />
                </div>
                <div className="col-4">
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                        <InputLabel>Hit Point Ability</InputLabel>
                        <Select
                            onClick={() => {setResultShown(false);}}
                            value={hitPointAbility}
                            label="Hit Point Ability"
                            
                            onChange={handleHitPointAbilityChange}
                        >
                            <MenuItem value={0}>Strength</MenuItem>
                            <MenuItem value={1}>Dexterity</MenuItem>
                            <MenuItem value={2}>Constitution</MenuItem>
                            <MenuItem value={3}>Intelligence</MenuItem>
                            <MenuItem value={4}>Wisdom</MenuItem>
                            <MenuItem value={5}>Charisma</MenuItem>

                        </Select>
                    </FormControl>

                </div>
            </div>
            <div className="weapon-selector border border-dark border-2 border-end-0 border-start-0">
            {selectedWeapons.map((weapon) => {
                return(
                <div key={weapon.id} className="row">
                <div className="col-2">
                <Autocomplete
                    onChange={(event, newValue) => {
                        let tempWeapons = [...selectedWeapons];
                        for (var i = 0; i < tempWeapons.length; i++) {
                            if (tempWeapons[i].id == weapon.id) {
                                tempWeapons[i].weapon1 = newValue; 
                                break;
                            }
                        }

                        setSelectedWeapons(tempWeapons);
                    }}
                    value={weapon.weapon1}
                    options={weapons}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Weapon 1"
                    />
                    )}
                /> 
                </div>
                <div className="m-4 text-center col-1">
                    OR
                </div>
                <div className="col-2">
                <Autocomplete
                    onChange={(event, newValue) => {
                        let tempWeapons = [...selectedWeapons];
                        for (var i = 0; i < tempWeapons.length; i++) {
                            if (tempWeapons[i].id == weapon.id) {
                                tempWeapons[i].weapon2 = newValue; 
                                break;
                            }
                        }

                        setSelectedWeapons(tempWeapons);
                    }}
                    value={weapon.weapon2}
                    options={weapons}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Weapon 2"
                    />
                    )}
                /> 
                </div>
                <div className="col-2 p-2">
                    <Button variant="outlined" onClick={() => {
                        setSelectedWeapons([...selectedWeapons, {id:weaponSelectorNextId++, weapon1:null,weapon2:null}]);
                    }}>+</Button>
                    <Button className="m-2" variant="outlined" onClick={() => {
                        if(selectedWeapons.length != 1){
                            setSelectedWeapons(selectedWeapons.filter(item => item.id != weapon.id));
                        };
                    }}>-</Button>
                </div>
            </div>);
            })}
            </div>
            <div className="row">
                <div className="col-7">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Additional Abilities" 
                        variant="outlined" value={additionalAbilities} onChange={handleAdditionalAbilitiesChange} fullWidth/> 
                </div>
            </div>
            <div className="row col-4">
                <img src={image}/>
            </div>
            <div className="row">
                <div className="col-3 float-left">
                    <Button variant="text" onClick={() => {resetInputs(); setResultShown(false); setEditing(false);}}>Cancel</Button>
                </div>
                <div className="col-3 float-left">
                    <Button variant="text" onClick={sendOnClick}>Send</Button>
                </div>
                <div className="col-3 float-left">
                    <Button variant="text" onClick={handleEditButton}>Edit</Button>
                </div>
                <div className="col-3 float-left">
                    <Button variant="text" onClick={removeOnClick}>Remove</Button>
                </div>
            </div>
            <div className="row">
                {isResultShow &&<AdminResult result={isSuccessfull} errorDescription={errorDescription}/>}
            </div>

            <hr/>

            <div style={{ height: 400, width: '100%' }}>
      <DataGrid sx={{
        boxShadow:2,
        border: 2,
        '& .MuiDataGrid-columnSeparator':{
            color: 'black'
        }
        }}
        onSelectionModelChange={(newSelectionModel) => {
            setSelectionModel(newSelectionModel);
        }}
        selectionModel={selectionModel}
        rows={classes}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}