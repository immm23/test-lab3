import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";
import TextField from "@mui/material/TextField";
import { FormControl, Select } from "@mui/material";
import ResultAdmin from "./ResultAdmin";
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
    {field: 'description', headerName: 'Description', width: 150 },
    {field: 'raceBonus', headerName: 'Subrace Bonus', width: 150, valueGetter: (params) => {
        return params.row.raceBonus.ability.name + " + " +  params.row.raceBonus.amount;
    }},
    {field: 'specialAbility', headerName: 'Speacial Ability', width: 150 }
];

export default function Subrace(){

    const [races, setRaces] = useState([]);
    const [subraces, setSubraces] = useState([]);
    const [selectedRace, setSelectedRace] = useState(0);
    const [selectedRaceInputValue, setSelectedRaceInputValue] = useState(0);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);
    const [raceBonusAbility, setRaceBonusAbility] = useState(0);
    const [raceBonusValue, setRaceBonusValue] = useState(0);
    const [specialAbility, setSpecialAbility] = useState('');
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

    const handleSpecialAbilityChange = (event) => {
        setSpecialAbility(event.target.value);
    }

    const handleRaceBonusValueChange = (event) => {
        setRaceBonusValue(event.target.value);
    }

    const handleRaceBonusAbilityChange = (event) =>{
        setRaceBonusAbility(event.target.value);
    }  

    const handleNameChange = (event) => {
        setName(event.target.value);
    }

    const handleDescriptionChange = (event) => {
        setDescription(event.target.value);
    }

    const resetInputs = (async) => {
        setName('');
        setDescription('');
        setRaceBonusAbility(0);
        setRaceBonusValue(0);
        setSpecialAbility('');
        setImage();
    }

    const removeOnClick = async() => {
        if(selectionModel.length > 0){
            selectionModel.forEach(index => {
                fetch(apiURL + '/subraces/delete/' +  index,{
                    method: 'DELETE',
                    headers:{'Authorization': 'Bearer '+ token}
                })
                .then((response) => {
                    setSusccesfull(response.status == 200);
                    setEditing(false);
                    setResultShown(true);
                    updateSubraces();
                })
                .catch((error) => setError(error))});      
    }   
      
    }
    const updateSubraces = async() => {
        await fetch(apiURL + '/subraces/all',{
            method: 'GET'
        })
        .then((responce) => {
            return responce.json();
        })
        .then((data) =>{
            setSubraces(data);
        })
        .catch((error) => console.log(error));
    }

    const sendOnClick = async() => {
        if(isEditing){
            let data = JSON.stringify({
                id: parseInt(selectionModel),
                name: name,
                raceBonus: {
                    abilityId: raceBonusAbility,
                    amount: raceBonusValue
                },
                specialAbility: specialAbility,
                description: description
            });


            await fetch(apiURL + '/subraces/update',{
                method: 'PUT',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: data
            })
            .then((response) => {
                setSusccesfull(response.status == 200);
                setEditing(false);
                setResultShown(true); 
                updateSubraces();   
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
            
            let generated = generatePath();

            let data = JSON.stringify({
                name: name,
                raceBonus: {
                    abilityId: raceBonusAbility,
                    amount: raceBonusAbility
                },
                specialAbility: specialAbility,
                description: description,
                raceId: selectedRace,
                imagePath: generated
            });
            
            await fetch(apiURL + '/subraces/new', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: data
            })
            .then((response) => {
                setSusccesfull(response.status == 201);
                setResultShown(true);
                updateSubraces();
    
            })
            .catch((error) => setError(error));

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
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = subraces.filter(function(x) {return x.id == selectionModel});
            console.log(filtered);
            setName(filtered[0].name);
            setDescription(filtered[0].description);
            setRaceBonusAbility(filtered[0].raceBonus.ability.abilityId);
            setRaceBonusValue(filtered[0].raceBonus.amount);
            setSpecialAbility(filtered[0].specialAbility);
            setPath(filtered[0].imagePath);
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
            <div>
                <div className="col-4">
                <Autocomplete
                    onChange={(event, newValue) => {
                        if(newValue == null){
                            setSelectedRace(null);
                        }
                        else{
                            setSelectedRace(newValue.id);
                        }

                        updateSubraces();
                    }}
                    onInputChange={(event, newInputValue) => {
                        setSelectedRaceInputValue(newInputValue.id);
                        updateSubraces();
                    }}
                    options={races}
                    getOptionLabel={(option) => option.name}
                    renderInput={(params) => (
                    <TextField
                        {...params}
                        size="small"
                        variant="outlined"
                        margin="normal"
                        label="Race"
                        placeholder="Race name"
                    />
                    )}
                />
                </div>
            </div>
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small"  label="Name" variant="outlined"
                margin="normal" onChange={handleNameChange} value={name}/> 
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Race Bonus Value" 
                    maxlines={4} variant="outlined" 
                    onChange={handleRaceBonusValueChange} value={raceBonusValue}/>                             
            </div>
            <div className="col-4">
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                        <InputLabel id="demo-simple-select-label">Race Bonus Ability</InputLabel>
                        <Select
                            onClick={() => {setResultShown(false);}}
                            labelId="demo-simple-select-label"
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
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Special Ability" 
                        variant="outlined" multiline maxlines={4}
                        onChange={handleSpecialAbilityChange} value={specialAbility}/>
                </div>
                <div className="col-4">
                <Button sx={{ m: 1}} variant="outline" component="label"
                    >
                    Upload File
                    <input
                    onChange={handleImageChange}
                    type="file"
                    hidden
                    />
                    </Button>

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
                {isResultShow &&<ResultAdmin result={isSuccessfull} errorDescription={errorDescription}/>}
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
        rows={subraces.filter(subrace => subrace.raceId == selectedRace)}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}