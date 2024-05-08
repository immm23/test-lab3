import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import StepButton from "./StepButton";
import { useEffect, useState, useContext} from "react";
import {apiURL} from "./APIURL";
import TextField from "@mui/material/TextField";
import { ColorRing } from 'react-loader-spinner';
import Autocomplete from '@mui/material/Autocomplete';
import {Context} from "./Context";

export default function RaseMenu(props){
    const context = useContext(Context);

    const [isLoading, setIsLoading] = useState(true);
    const [isSelected, setIsSelected] = useState(false);
    const [races, setRaces] = useState([]);
    const [raceValue, setRaceValue] = useState();
    const [raceInputValue, setRaceInputValue] = useState('');

    const [description, setDescription] = useState('');
    const [age, setAge] = useState('');
    const [alignment, setAlignment] = useState('');
    const [size, setSize] = useState('');
    const [speed, setSpeed] = useState('');
    const [raceBonus, setRaceBonus] = useState('');
    const [toolBonus, setToolBonus] = useState('');
    const [darkSight, setDarkSight] = useState('');
    const [languages, setLanguages] = useState('');
    const [additionalInfo, setAdditionalInfo] = useState('');
    const [image, setImage] = useState();

    const[rerenderTrigger, setRerenderTrigger] = useState(false);

    const handleAutocompleteChange = (event, newValue) =>{
        if (newValue == null) {
            setIsSelected(false);
            setRaceInputValue('');
        } 
        else {
            setIsSelected(true);
            updateData(newValue);
            setRaceInputValue(newValue.name);
        }
        setRaceValue(newValue);
    }

    useEffect(() => {
        console.log("tr");
        console.log(apiURL);
        fetch(apiURL +'/races/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setRaces(data);

            if(context.raceId != null){
                const defaultRace = data.find(race => race.id == context.raceId)
                setRaceValue(defaultRace);
                updateData(defaultRace);    
                setRaceInputValue(defaultRace.name);
                setIsSelected(true);
            }
        })
        .catch((error) => console.log(error));
    }, []);

    const updateData = async(newValue) => {
        await setIsLoading(true);
        
        await fetch(apiURL +'/images/' + newValue.imagePath  + '/get', {
            method: 'GET'
            })
            .then((response) => {
                return response.blob();
            })
            .then((data) => {
                setImage(URL.createObjectURL(data));
            })
            .catch((error) => console.log(error));
        
            await setDescription(newValue.description);
            await setAge(newValue.ageDescription);
            await setSize(newValue.sizeDescription);
            await setSpeed(newValue.speed);
            await setToolBonus(newValue.toolsBonus);

            let additionaLanguages;
            if(newValue.additionalLanguages > 0){
                additionaLanguages = ' + ' + newValue.additionalLanguages + ' more';
            }
            else {
                additionaLanguages = '';
            }
            await setLanguages(newValue.nativeLanguages.map(a => a.name).toString() + additionaLanguages);
            await setRaceBonus(newValue.raceBonus.ability.name + ' + ' + newValue.raceBonus.amount);
            await setDarkSight(newValue.darkVision);
            await setAdditionalInfo(newValue.additionalBonus);
            await setAlignment(newValue.alignment.alignmentStrength.name + '-' + newValue.alignment.alignmentSide.name);
            
            context.raceId = newValue.id;
            context.subracesCount = 0;
            context.subraceId = null;

            await setIsLoading(false);
            await setRerenderTrigger(!rerenderTrigger);
    }
    
    return(
        <div className="race-menu text-center">
            <div className="d-flex justify-content-between">
                <StepButton text="Back" onClick={()=> {props.this(false); props.previous(true);}} />
                <Autocomplete
                 key={rerenderTrigger}
                    sx={{ width: 200 }}
                    size="small"
                    id="combo-box-demo"
                    getOptionLabel={(option) => option.name}
                    options={races}
                    value = {raceValue}
                    inputValue = {raceInputValue}
                    onChange= {handleAutocompleteChange}
                    renderInput={(params) => <TextField size="small" {...params} label="Race" />}/>
                <StepButton disabled={!isSelected || isLoading} text="Next" onClick={()=> {props.this(false); props.next(true); }}/>
            </div>
            {!isSelected && <div className="d-flex justify-content-center p-5">
                <h3>Select your race!</h3>
                </div>
            }
            {!isLoading && isSelected &&
                <div>
                <div className="race-description row">
                    <div className="p-2 image col-4">
                        <img className="image" src={image} alt="character"/>
                    </div>
                <div className="col-8">
                <div className="final-long border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Description</h4></div><br/></strong>
                        <p className="item-final">{description}</p>
                        </p>
                    </div>
                    <div className="row">
                        <div className="col">
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Age</h4></div><br/></strong>
                            <p className="item-final">{age}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Ideology</h4></div><br/></strong>
                        <p className="item-final">{alignment}</p>
                        </p>
                    </div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Size</h4></div><br/></strong>
                        <p className="item-final">{size}</p>
                        </p>
                    </div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Speed</h4></div><br/></strong>
                        <p className="item-final">{speed}</p>
                        </p>
                    </div>
                        </div>
                        <div className="col">
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Dark sight</h4></div><br/></strong>
                        <p className="item-final">{darkSight}</p>
                        </p>
                    </div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Lanugages</h4></div><br/></strong>
                        <p className="item-final">{languages}</p>
                        </p>
                    </div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Race bonus</h4></div><br/></strong>
                        <p className="item-final">{raceBonus}</p>
                        </p>
                    </div>
                    <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Tools bonus</h4></div><br/></strong>
                        <p className="item-final">{toolBonus}</p>
                        </p>
                    </div>

                        </div>
                    </div>
                    <div className="row ps-2">
                    <div className="final-long border rounded-2 border-2 border-dark p-2 m-2">
                        <p>
                            <strong>
                        <div className="border-2 border-bottom border-dark"><h4>Additional info</h4></div><br/></strong>
                        <p className="item-final">{additionalInfo}</p>
                        </p>
                    </div>
                </div>          
                </div>
                </div>
            </div>
            }

            {isLoading && isSelected &&
                <div className="d-flex justify-content-center">
                    <ColorRing
                        visible={true}
                        height="200"
                        width="200"
                        ariaLabel="blocks-loading"
                        wrapperStyle={{}}
                        wrapperClass="blocks-wrapper"
                        colors={['#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0']}
                        />
                </div>
            }                
  
        </div>
    );
}