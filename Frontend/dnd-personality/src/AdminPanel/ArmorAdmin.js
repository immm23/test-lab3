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
import { useAuth0 } from "@auth0/auth0-react";

import {apiURL} from "../APIURL";

const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Name', width: 150 },
    {field: 'armorClass', headerName: 'Armor Class', width: 250, valueGetter: (params) => {
        if( params.row.bonusAbility == null){
            return params.row.basePoints;
        }
        else{
            return params.row.basePoints  + " + " + params.row.bonusAbility.name + " modificator";
        }
    }},
    {field: 'armorTypeId', headerName: 'Armor Type', width: 150, valueGetter: (params) => {
        return params.row.armorType.name;
    }},
    {field: 'price', headerName: 'Price', width: 150 }
];

export default function ArmorAdmin(){

    const [languages, setLanguages] = useState([]);
    const [name, setName] = useState('');
    const [price, setPrice] = useState(0);
    const [armorClass, setArmorClass] = useState(0);
    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);
    const [armorType, setArmorType] = useState(0);
    const [bonusAbility, setBonusAbility] = useState(7);
    const [token, setToken] = useState(null);
    const {getAccessTokenSilently} = useAuth0();

    const handleArmorTypeChange = (event) => {
        setArmorType(event.target.value);
    };    
    
    const handleBonusAbilityChange = (event) => {
        setBonusAbility(event.target.value);
    };    

    const handleNameChange = (event) => {
        setName(event.target.value);
    }

    const handlePriceChange = (event) => {
        setPrice(event.target.value);
    }

    const handleArmorClassChange = (event) => {
        setArmorClass(event.target.value);
    }

    const resetInputs = (async) => {
        setName('');
        setArmorClass('');
        setPrice(0);
        setArmorType(0);
        setBonusAbility(7);
    }

    
    useEffect(() => {
        (async () => {
          const accessToken = await getAccessTokenSilently();
          setToken(accessToken);
        })();
      }, []);

const removeOnClick = async() => {
    if(selectionModel.length > 0){
        selectionModel.forEach(index => {
            fetch(apiURL + '/armors/delete/' +  index,{
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
        if(isEditing){
            let data = '';
            if(bonusAbility == 7){
                data = JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    bonusAbilityId: null,
                    armorTypeId: armorType,
                    basePoints: armorClass,
                    id: parseInt(selectionModel)
                })
            }
            else{
                data = JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    armorTypeId: armorType,
                    basePoints: armorClass,
                    bonusAbilityId: bonusAbility,
                    id: parseInt(selectionModel)
                })
            }

            await fetch(apiURL + '/armors/update',{
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
        }
        else{

            let data = '';
            
            if(bonusAbility == 7){
                data = JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    armorTypeId: armorType,
                    basePoints: armorClass,
                    bonusAbilityId: null
                })
            }
            else{
                data = JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    armorTypeId: armorType,
                    basePoints: armorClass,
                    bonusAbilityId: bonusAbility,
                })
            }

            await fetch(apiURL + '/armors/new', {
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
            .catch((error) => setError(error));
        }
    };

    useEffect(() => {
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = languages.filter(function(x) {return x.id == selectionModel})
            setName(filtered[0].name);
            setPrice(filtered[0].price);
            setArmorClass(filtered[0].basePoints);
            setArmorType(filtered[0].armorTypeId);
            if(filtered[0].bonusAbility != null){
                setBonusAbility(filtered[0].bonusAbilityId);
            }
            setEditing(true);
        }
    }

    const loadData = async() => {
        await fetch(apiURL +'/armors/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setLanguages(data);
        })
        .catch((error) => console.log(error));
    }

    return(
        <div className="accordeon-content-inside row container">
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small"  label="Name" variant="outlined"
                margin="normal" onChange={handleNameChange} value={name}/>
                <TextField onClick={() => {setResultShown(false);}} size="small" margin="normal" label="Price (optional)" variant="outlined"
                onChange={handlePriceChange} value={price}/>  
                <TextField onClick={() => {setResultShown(false);}} margin="normal" size="small" label="Armor strength" variant="outlined"
                onChange={handleArmorClassChange} value={armorClass}/>                                    
            </div>
            <div className="col-4">
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                    <InputLabel id="demo-simple-select-label">Armor Type</InputLabel>
                    <Select
                        onClick={() => {setResultShown(false);}}
                        labelId="demo-simple-select-label"
                        value={armorType}
                        label="Armor Type"
                        onChange={handleArmorTypeChange}
                    >
                        <MenuItem value={0}>Light Armor</MenuItem>
                        <MenuItem value={1}>Heavy Armor</MenuItem>
                        <MenuItem value={2}>Medium Armor</MenuItem>
                        <MenuItem value={3}>Shield</MenuItem>
                    </Select>
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                    <InputLabel id="demo-simple-select-label">Bonus Ability Type</InputLabel>
                    <Select
                        onClick={() => {setResultShown(false);}}
                        labelId="demo-simple-select-label"
                        value={bonusAbility}
                        label="Armor Type"
                        onChange={handleBonusAbilityChange}
                    >
                        <MenuItem value={7}>None</MenuItem>
                        <MenuItem value={0}>Strength</MenuItem>
                        <MenuItem value={1}>Dexterity</MenuItem>
                        <MenuItem value={2}>Constitution</MenuItem>
                        <MenuItem value={4}>Intelligence</MenuItem>
                        <MenuItem value={5}>Wisdom</MenuItem>
                        <MenuItem value={6}>Charisma</MenuItem>

                    </Select>
                </FormControl>
            </div>
            <div className="row">
                <div className="col-4">
                
                </div>
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
        rows={languages}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}