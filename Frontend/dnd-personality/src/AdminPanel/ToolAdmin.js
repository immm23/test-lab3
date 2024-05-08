import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";
import TextField from "@mui/material/TextField";
import ResultAdmin from "./ResultAdmin";
import { DataGrid } from '@mui/x-data-grid';
import { useEffect } from "react";
import React, { useState } from "react";
import Button  from "@mui/material/Button";
import {apiURL} from "../APIURL";
import { useAuth0 } from "@auth0/auth0-react";


const columns = [
    {field: 'id', headerName: 'ID', width:70},
    {field: 'name', headerName: 'Tool', width: 150 },
    {field: 'price', headerName: 'Price', width: 150 }
];

export default function ToolAdmin(){

    const [tools, setTools] = useState([]);
    const [name, setName] = useState('');
    const [price, setPrice] = useState(0);
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
                fetch(apiURL + '/tools/delete/' +  index,{
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
            await fetch(apiURL +'/tools/update',{
                method: 'PUT',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token },
                body: JSON.stringify({
                    name: name,
                    price: price,
                    id: parseInt(selectionModel)
                })
            })
            .then((response) => {
                setSusccesfull(response.status == 200);
                setEditing(false);
                setResultShown(true);
                setName('');
                setPrice(0);
                loadData();
    
            })
            .catch((error) => {
                setError(error);
                setEditing(false);
            });
        }
        else{
            await fetch(apiURL +'/tools/new',{
                method: 'POST',
                headers: { 'Content-Type': 'application/json',
                'Authorization': 'Bearer '+ token },
                body: JSON.stringify({
                    name: name,
                    price: price
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

    const handlePriceChange = (event) => {
        setPrice(event.target.value);
    }

    useEffect(() => {
        loadData();
    }, []);


    const handleEditButton = (event) => {
        if(selectionModel.length == 1){
            let filtered = tools.filter(function(x) {return x.id == selectionModel})
            setName(filtered[0].name);
            setEditing(true);
        }
    }

    const loadData = async() => {
        fetch(apiURL +'/tools/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setTools(data);
        })
        .catch((error) => console.log(error));
    }

    return(
        <div className="accordeon-content-inside row container">
            <div className="col-4">
                <TextField onClick={() => {setResultShown(false);}} size="small" id="outlined-basic" label="Tool" variant="outlined"
                margin="normal" onChange={handleTextChange} value={name}/>
                <TextField onClick={() => {setResultShown(false);}} size="small" id="outlined-basic" label="Price" variant="outlined"
                margin="normal" onChange={handlePriceChange} value={price}/>
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
        rows={tools}
        columns={columns}
        pageSize={5}
        rowsPerPageOptions={[5]}
        checkboxSelection
      />
    </div>
        </div>
    );
}