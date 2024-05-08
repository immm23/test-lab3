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

const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Name', width: 150 },
    {field: 'description', headerName: 'Description', width: 250 },
    {field: 'names', headerName: 'Names', width: 250, valueGetter: (params) => {
        let names = params.row.names.map(a => a.raceName);
        return names;
    }},
    {field: 'raceBonus', headerName: 'Rase Bonus', width: 150, valueGetter: (params) => {
        return params.row.raceBonus.ability.name + " + " +  params.row.raceBonus.amount;
    }},
    {field: 'ageDescription', headerName: 'Age', width: 150 },
    {field: 'alignment', headerName: 'Alignment', width: 150, valueGetter: (params) => {
        return params.row.alignment.alignmentStrength.name+'-'+ params.row.alignment.alignmentSide.name;
    } },
    {field: 'sizeDescription', headerName: 'Size', width: 150 },
    {field: 'speed', headerName: 'Speed', width: 150 },
    {field: 'nativeLanguages', headerName: 'Native Languages', width: 150, valueGetter: (params) => {
        let languages =params.row.nativeLanguages.map(a => a.name) ;
        return languages;
    }},
    {field: 'additionalLanguages', headerName: 'Additional Languages', width: 150},
    {field: 'darkVision', headerName: 'Dark Vision', width: 150},
    {field: 'toolsBonus', headerName: 'Tools Bonus', width: 150 },
    {field: 'additionalBonus', headerName: 'Aditional Bonus', width: 150 }
];

export default function RaceAdmin(){
    const [languages, setLanguages] = useState([]);
    const [races, setRaces] = useState([]);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);
    const [alignmentSide, setAlignmentSide] = useState(0);
    const [alignmentStrength, setAlignmentStrength] = useState(0);
    const [sendNames, setSendNames] = useState([]);
    const [age, setAge] = useState('');
    const [raceBonusAbility, setRaceBonusAbility] = useState(0);
    const [raceBonusValue, setRaceBonusValue] = useState(0);
    const [speed, setSpeed] = useState(0);
    const [darkVision, setDarkVision] = useState('');
    const [size, setSize] = useState('');
    const [toolBonus, setToolBonus] = useState('');
    const [additionaLanguages, setAdditionalLanguages] = useState(0);
    const [additionalInfo, setAdditionalInfo] = useState('');
    const [image, setImage] = useState();
    const [imageFile, setImageFile] = useState();
    const [path, setPath] = useState('');

    const [defaultLanguageValue, setDefaultLanguageValue] = useState([]);
    const [defaultLanguageInputValue, setDefaultLanguageInputValue] = useState([]);

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

    const handleAdditionalInfoChange = (event) => {
        setAdditionalInfo(event.target.value);
    }

    const handleAdditionalLanguageChange = (event) => {
        setAdditionalLanguages(event.target.value);
    }

    const handleToolBonusChange = (event) => {
        setToolBonus(event.target.value);
    }

    const handleSizeChange = (event) => {
        setSize(event.target.value);
    }

    const handleDarkVisionChange = (event) => {
        setDarkVision(event.target.value);
    }

    const handleSpeedChange = (event) => {
        setSpeed(event.target.value);
    }

    const handleRaceBonusValueChange = (event) => {
        setRaceBonusValue(event.target.value);
    }

    const handleRaceBonusAbilityChange = (event) =>{
        setRaceBonusAbility(event.target.value);
    }

    const handleAgeChange = (event) => {
        setAge(event.target.value);
    }

    const handleAlignmentSideChange = (event) => {
        setAlignmentSide(event.target.value);
    };    
    
    const handleAlignmentStrengthChange = (event) => {
        setAlignmentStrength(event.target.value);
    };    

    const handleNameChange = (event) => {
        setName(event.target.value);
    }

    const handleNamesChange = (event) => {
        let array = event.target.value.split(",");
        setSendNames(array);
    }

    const handleDescriptionChange = (event) => {
        setDescription(event.target.value);
    }

    const resetInputs = (async) => {
        setImage();
        setImageFile();
        setName('');
        setDarkVision('');
        setRaceBonusAbility(0);
        setRaceBonusValue(0);
        setAlignmentSide(0);
        setAlignmentStrength(0);
        setSendNames([]);
        setDescription('');
        setAge('');
        setSize('');
        setSpeed(0);
        setToolBonus('');
        setAdditionalLanguages(0);
        setAdditionalInfo('');
        setDefaultLanguageValue([]);
        setDefaultLanguageInputValue([]);
    }

    
    const removeOnClick = async() => {
        if(selectionModel.length > 0){
            selectionModel.forEach(raceId => {
                fetch(apiURL + '/images/' + races.filter(function(race){
                    return race.id == raceId
                }).imagePath +  ' /delete',{
                    method: 'DELETE',
                    headers: { 'Content-Type': 'application/json',
                    'Authorization': 'Bearer '+ token}
                });
                fetch(apiURL + '/races/delete/' +  raceId,{
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
                id: selectionModel[0],
                name: name,
                names: sendNames.map(a => ({raceName: a})),
                raceBonus: {
                    abilityId: raceBonusAbility,
                    amount: raceBonusAbility
                },
                ageDescription: age,
                alignment: {
                    alignmentSideId : alignmentSide,
                    alignmentStrengthId : alignmentStrength
                },
                sizeDescription: size,
                darkVision: darkVision,
                description: description,
                speed : speed,
                nativeLanguages: defaultLanguageValue,
                additionalLanguages: additionaLanguages,
                toolsBonus: toolBonus,
                additionalBonus: additionalInfo    
            });
            await fetch(apiURL + '/races/update',{
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
                names: sendNames.map(a => ({raceName: a})),
                raceBonus: {
                    abilityId: raceBonusAbility,
                    amount: raceBonusAbility
                },
                ageDescription: age,
                alignment: {
                    alignmentSideId : alignmentSide,
                    alignmentStrengthId : alignmentStrength
                },
                sizeDescription: size,
                darkVision: darkVision,
                description: description,
                speed : speed,
                nativeLanguages: defaultLanguageValue,
                additionalLanguages: additionaLanguages,
                toolsBonus: toolBonus,
                additionalBonus: additionalInfo,
                imagePath: generated
    
            });

            await fetch(apiURL + '/races/new',{
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
        fetch(apiURL +'/languages/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setLanguages(data);
        })
        .catch((error) => console.log(error));

        loadData();
    }, []);

    useEffect(() => {
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = races.filter(function(x) {return x.id == selectionModel});
            setPath(filtered[0].imagePath);
            setName(filtered[0].name);
            setSendNames(filtered[0].names.map(a => a.raceName));
            setDescription(filtered[0].description);
            setAlignmentSide(filtered[0].alignment.alignmentSide.alignmentSideId);
            setAlignmentStrength(filtered[0].alignment.alignmentStrength.alignmentStrengthId);
            setDarkVision(filtered[0].darkVision);
            setRaceBonusAbility(filtered[0].raceBonus.ability.abilityId);
            setRaceBonusValue(filtered[0].raceBonus.amount);
            setSize(filtered[0].sizeDescription);
            setToolBonus(filtered[0].toolsBonus);
            setAdditionalLanguages(filtered[0].additionalLanguages); 
            setAdditionalInfo(filtered[0].additionalBonus);
            setSpeed(filtered[0].speed);
            setAge(filtered[0].ageDescription);
            setEditing(true);
            setDefaultLanguageInputValue(filtered[0].nativeLanguages);
            setDefaultLanguageValue(filtered[0].nativeLanguages.map((itemObj) =>
            {return languages.find(option => option.id === itemObj.id)}));
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
        await fetch(apiURL +'/races/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setRaces(data);
        })
        .catch((error) => console.log(error));
    }

    return(
        <div className="accordeon-content-inside row container">
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small"  label="Name" variant="outlined"
                margin="normal" onChange={handleNameChange} value={name}/>
                
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Dark Vision" 
                        variant="outlined" 
                        onChange={handleDarkVisionChange} value={darkVision}/>   
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Race Bonus Value" 
                    maxlines={4} variant="outlined" 
                    onChange={handleRaceBonusValueChange} value={raceBonusValue}/>                             
            </div>
            <div className="col-4">
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                    <InputLabel>Alignment Side</InputLabel>
                    <Select
                        onClick={() => {setResultShown(false);}}
                        value={alignmentSide}
                        label="Armor Type"
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
                        onClick={() => {setResultShown(false);}}
                        value={alignmentStrength}
                        label="Armor Type"
                        onChange={handleAlignmentStrengthChange}
                    >
                        <MenuItem value={0}>Lawful</MenuItem>
                        <MenuItem value={1}>Neutal</MenuItem>
                        <MenuItem value={2}>Chaotic</MenuItem>
                    </Select>
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                        <InputLabel>Race Bonus Ability</InputLabel>
                        <Select
                            onClick={() => {setResultShown(false);}}
                            value={raceBonusAbility}
                            label="Armor Type"
                            
                            onChange={handleRaceBonusAbilityChange}
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
            <div className="row">
                <div className="col-7">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Description" multiline 
                    maxlines={4} variant="outlined" fullWidth
                    onChange={handleDescriptionChange} value={description}/>  
                </div>
            </div>
            <div className="row">
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Names" multiline 
                        maxlines={4} variant="outlined" 
                        onChange={handleNamesChange} value={sendNames}/>  
                </div>
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Age" variant="outlined" 
                        onChange={handleAgeChange} value={age}/>  
                </div>
            </div>
            <div className="row">
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Size" 
                        variant="outlined" 
                        onChange={handleSizeChange} value={size}/>
                </div>
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Speed" 
                        variant="outlined" 
                        onChange={handleSpeedChange} value={speed}/>
                </div>
            </div>
            <div className="row">

                <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Tools Bonuses" 
                        variant="outlined" multiline maxlines={4}
                        onChange={handleToolBonusChange} value={toolBonus}/>
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
                <Autocomplete
                    multiple
                    onChange={(event, newValue) => {
                        setDefaultLanguageValue(newValue);
                    }}
                    onInputChange={(event, newInputValue) => {
                        setDefaultLanguageInputValue(newInputValue);
                    }}
                    value={defaultLanguageValue}
                    inputValue={defaultLanguageInputValue}
                    options={languages}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Race languages"
                        placeholder="Languages"

                    />
                    )}
                />
                </div>
                <div className="col-4">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Additional Languages Count" 
                        variant="outlined"
                        onChange={handleAdditionalLanguageChange} value={additionaLanguages}/> 
                </div>
            </div>
            <div className="row">
                <div className="col-7">
                    <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Additional Info" 
                        variant="outlined" fullWidth
                        onChange={handleAdditionalInfoChange} value={additionalInfo}/> 
                </div>
            </div>
            <div className="row col-4">
                <img src={image} />
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
        rows={races}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}