import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";
import TextField from "@mui/material/TextField";
import AdminResult from "./ResultAdmin";
import { DataGrid } from '@mui/x-data-grid';
import { useEffect } from "react";
import React, { useState } from "react";
import Button  from "@mui/material/Button";
import {apiURL} from "../APIURL";
import { useAuth0 } from "@auth0/auth0-react";


const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Item', width: 150 }
];

export default function ItemAdmin(){

    const [items, setItems] = useState([]);
    const [name, setName] = useState('');
    const [isResultShow, setResultShown] = useState(false);
    const [isSuccessfull, setSusccesfull] = useState(false);
    const [errorDescription, setError] = useState('');
    const [selectionModel, setSelectionModel] = useState([]);
    const [isEditing, setEditing] = useState(false);
    const [token, setToken] = useState(null);
    const {getAccessTokenSilently} = useAuth0();


    useEffect(() => {
        (async () => {
          const accessToken = await getAccessTokenSilently();
          setToken(accessToken);
        })();
      }, []);

const removeOnClick = async() => {
        if(selectionModel.length > 0){
            selectionModel.forEach(index => {
                fetch(apiURL + '/items/delete/' +  index,{
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
            await fetch(apiURL +'/items/update',{
                method: 'PUT',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token },
                body: JSON.stringify({
                    name: name,
                    id: parseInt(selectionModel)
                })
            })
            .then((response) => {
                setSusccesfull(response.status == 200);
                setEditing(false);
                setResultShown(true);
                setName('');
    
                loadData();
    
            })
            .catch((error) => {
                setError(error);
                setEditing(false);
            });
        }
        else{
            await fetch(apiURL +'/items/new',{
                method: 'POST',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token },
                body: JSON.stringify({
                    name
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

    const handleTextChange = (event) => {
        setName(event.target.value);
    }

    useEffect(() => {
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = items.filter(function(x) {return x.id == selectionModel})
            setName(filtered[0].name);
            setEditing(true);
        }
    }

    const loadData = async() => {
        fetch(apiURL +'/items/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setItems(data);
        })
        .catch((error) => console.log(error));
    }

    return(
        <div className="accordeon-content-inside row container">
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small" id="outlined-basic" label="Item" variant="outlined"
                margin="normal" onChange={handleTextChange} value={name}/>
            </div>
            <div className="row">
                <div className="col-3 float-left">
                    <Button variant="text" onClick={() => {setName(''); setResultShown(false); setEditing(false);}}>Cancel</Button>
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
        rows={items}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}