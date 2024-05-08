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
import { useAuth0 } from "@auth0/auth0-react";

import {apiURL} from "../APIURL";

const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Name', width: 150 },
    {field: 'damage', headerName: 'Damage', width: 150 },
    {field: 'weaponTypeId', headerName: 'Weapon Type', width: 150, valueGetter: (params) => {
        return params.row.weaponType.name;
    }},
    {field: 'damageTypeId', headerName: 'Damage Type', width: 150, valueGetter: (params) => {
        return params.row.damageType.name;
    }},
    {field: 'price', headerName: 'Price', width: 150 }
];

export default function WeaponAdmin(){

    const [languages, setLanguages] = useState([]);
    const [name, setName] = useState('');
    const [price, setPrice] = useState(0);
    const [damage, setDamage] = useState('');
    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);
    const [weaponType, setWeaponType] = useState(0);
    const [damageType, setDamageType] = useState(0);

    const [token, setToken] = useState(null);
    const {getAccessTokenSilently} = useAuth0();

    useEffect(() => {
        (async () => {
          const accessToken = await getAccessTokenSilently();
          setToken(accessToken);
        })();
      }, []);

    const handleWeaponTypeChange = (event) => {
        setWeaponType(event.target.value);
    };    
    
    const handleDamageTypeChange = (event) => {
        setDamageType(event.target.value);
    };

    const handleNameChange = (event) => {
        setName(event.target.value);
    }

    const handlePriceChange = (event) => {
        setPrice(event.target.value);
    }

    const handleDamageChange = (event) => {
        setDamage(event.target.value);
    }

    const resetInputs = (async) => {
        setName('');
        setDamage('');
        setPrice(0);
        setWeaponType(0);
        setDamageType(0);
    }

    const removeOnClick = async() => {
        if(selectionModel.length > 0){
            selectionModel.forEach(index => {
                fetch(apiURL + '/weapons/delete/' +  index,{
                    method: 'DELETE',
                    headers: {'Authorization': 'Bearer '+ token}
                })
            .then((response) => {
                setSusccesfull(response.status == 200);
                setEditing(false);
                setResultShown(true);                
                loadData();
        
            })
            .catch((error) => setError(error));      
    })
      
    }};

    const sendOnClick = async() => {
        if(isEditing){
            await fetch(apiURL + '/weapons/update',{
                method: 'PUT',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    damageTypeId: damageType,
                    weaponTypeId: weaponType,
                    damage: damage,
                    id: parseInt(selectionModel)
                })
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
            await fetch(apiURL + '/weapons/new', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token},
                body: JSON.stringify({
                    name: name,
                    price: parseInt(price),
                    damageTypeId: damageType,
                    weaponTypeId: weaponType,
                    damage: damage
                })
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
            setDamage(filtered[0].damage);
            setWeaponType(filtered[0].weaponType.weaponTypeId);
            setDamageType(filtered[0].damageType.damageTypeId);
            setEditing(true);
        }
    }

    const loadData = async() => {
        await fetch(apiURL +'/weapons/all', {
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
                <TextField onClick={() => {setResultShown(false);}} margin="normal" size="small" label="Damage" variant="outlined"
                onChange={handleDamageChange} value={damage}/>                                    
            </div>
            <div className="col-4">
            <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                    <InputLabel id="demo-simple-select-label">Weapon Type</InputLabel>
                    <Select
                        onClick={() => {setResultShown(false);}}
                        labelId="demo-simple-select-label"
                        value={weaponType}
                        label="Weapon Type"
                        onChange={handleWeaponTypeChange}
                    >
                        <MenuItem value={0}>Simple Melee</MenuItem>
                        <MenuItem value={1}>Simple Ranged</MenuItem>
                        <MenuItem value={2}>Martial Melee</MenuItem>
                        <MenuItem value={3}>Martial Ranged</MenuItem>
                    </Select>
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }} size="small">
                    <InputLabel id="demo-simple-select-label">Damage Type</InputLabel>
                    <Select
                        onClick={() => {setResultShown(false);}}
                        labelId="demo-simple-select-label"
                        value={damageType}
                        label="Damage Type"
                        onChange={handleDamageTypeChange}
                    >
                        <MenuItem value={0}>Bludgeoning</MenuItem>
                        <MenuItem value={1}>Piercing</MenuItem>
                        <MenuItem value={2}>Slashing</MenuItem>
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