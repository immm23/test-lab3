import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";

import StepButton from "./StepButton";
import { useEffect, useState, useContext} from "react";
import {apiURL} from "./APIURL";
import TextField from "@mui/material/TextField";
import { ColorRing} from 'react-loader-spinner';
import Autocomplete from '@mui/material/Autocomplete';
import {Context} from "./Context";

export default function ClassMenu(props){
    const context = useContext(Context);

    const [isLoading, setIsLoading] = useState(true);
    const [isSelected, setIsSelected] = useState(false);
    const [image, setImage] = useState();

    const [classes, setClasses] = useState([]);
    const [classValue, setClassValue] = useState();
    const [classInputValue, setClassInputValue] = useState('');

    const [description, setDescription] = useState('');
    const [proficiencyBonus, setProficiencyBonus] = useState('');
    const [primaryAbility, setPrimaryAbility] = useState('');
    const [savingThrow, setSavingThrow] = useState('');
    const [hitDice, setHitDice] = useState('');
    const [hitPoints, setHitPoints] = useState('');
    const [armorTypes, setArmorTypes] = useState('');
    const [weaponTypes, setWeaponTypes] = useState('');
    const [tools, setTools] = useState('');
    const [skills, setSkills] = useState('');
    const [additionalAbilities, setAdditionalAbilities] = useState('');
    const [weaponSelectors, setWeaponSelectors] = useState('');
    const [startItemsDefault, setStartItemsDefault] = useState('');
    const [skillsToSelect, setSkillsToSelect] = useState('');
    const [rerenderTrigger, setRerenderTrigger] = useState(false);

    useEffect(() => {
        fetch(apiURL +'/classes/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            setClasses(data);

        if(context.classId != null){
            const defaultClass = data.find(item => item.id == context.classId);
            setClassValue(defaultClass);
            updateData(defaultClass);    
            setClassInputValue(defaultClass.name);
            setIsSelected(true);
            console.log('def');
            console.log(defaultClass);
        }
        
        })
        .catch((error) => console.log(error));
    }, []);

    const updateData = async(newValue) => {
        await setIsLoading(true);
        console.log(newValue);
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
            await setProficiencyBonus(newValue.proficiencyBonus);
            await setPrimaryAbility(newValue.primaryAbility.name);
            await setSavingThrow(newValue.savingThrow.name);
            await setHitDice(newValue.hitDice);
            await setTools(newValue.tools.map(a => a.name).join(', '));
            await setSkills(newValue.skills.map(a => a.name).join(', '));
            await setSkillsToSelect(newValue.selectedSkills + ' skills to be selected from list');
            await setHitPoints(newValue.hitPoints.ability.name + " + " + newValue.hitPoints.default);
            await setArmorTypes(newValue.armorTypes.map(a => a.name).join(', '));
            await setWeaponTypes(newValue.weaponTypes.map(a => a.name).join(', '));
            await setAdditionalAbilities(newValue.additionalAbilities);
            await setStartItemsDefault(newValue.startItems.map(a => a.name).join(', '));

            const items = newValue.weaponSelectors.map(
                (selector) =>
                  `${selector.weapons
                    .map((weapon) => `${weapon.name}`)
                    .join(" or ")}`
              )
              .join(", ");

            await setWeaponSelectors(items);
            console.log('new vale' + newValue.id);
            context.classId = newValue.id;

            await setIsLoading(false);
            await setRerenderTrigger(!rerenderTrigger);
    }
    
    return(
        <div className="race-menu text-center">
            <div className="d-flex justify-content-between">
                <StepButton text="Back" onClick={()=> 
                    {
                        //is there is any subrace - return to subrace select. Else - to race select
                        if(context.subracesCount != null ){
                            props.previous[1](true);
                        }
                        else{
                            props.previous[0](true);
                        }
                        props.this(false);
                    }} />
                <Autocomplete
                    key={rerenderTrigger}
                    sx={{ width: 200 }}
                    size="small"
                    id="combo-box-demo"
                    getOptionLabel={(option) => option.name}
                    options={classes}
                    value ={classValue}
                    inputValue={classInputValue}
                    onChange={(event, newValue) => {

                       if(newValue == null){
                        setIsSelected(false);
                        setClassInputValue('');
                       }
                       else {
                        setIsSelected(true);
                        updateData(newValue);
                        setClassInputValue(newValue.name);
                       }
                        setClassValue(newValue);
                    }}
                    renderInput={(params) => <TextField size="small" {...params} label="Class" />}/>
                <StepButton disabled={!isSelected || isLoading} text="Next" onClick={()=> {props.this(false); props.next(true); }}/>
            </div>
            {!isSelected && <div className="d-flex justify-content-center p-5">
                <h3>Select your class!</h3>
                </div>
            }
            {!isLoading && isSelected &&
                <div>
                <div className="class-description row">
                    <div className="image col-4 p-2">
                        <img className="image" src={image} alt="character"/>
                    </div>
                <div className="col-8">
                    <div>
                    <div className="final-long border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Description</h4></div><br/></strong>
                            <p className="item-final">{description}</p>
                            </p>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Saving throw</h4></div><br/></strong>
                            <p className="item-final">{savingThrow}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Hit dice</h4></div><br/></strong>
                            <p className="item-final">{hitDice}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Hit points</h4></div><br/></strong>
                            <p className="item-final">{hitPoints}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Armor types</h4></div><br/></strong>
                            <p className="item-final">{armorTypes}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Weapon types</h4></div><br/></strong>
                            <p className="item-final">{weaponTypes}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Tools</h4></div><br/></strong>
                            <p className="item-final">{tools}</p>
                            </p>
                        </div>
                        </div>
                        <div className="col">
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Primary ability</h4></div><br/></strong>
                            <p className="item-final">{primaryAbility}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Proficiency bonus</h4></div><br/></strong>
                            <p className="item-final">{proficiencyBonus}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Skills</h4></div><br/></strong>
                            <p className="item-final">{skills}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Start items</h4></div><br/></strong>
                            <p className="item-final">{startItemsDefault}</p>
                            </p>
                        </div>
                        <div className="final-small border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Weapons</h4></div><br/></strong>
                            <p className="item-final">{weaponSelectors}</p>
                            </p>
                        </div>

                        </div>
                    </div>
                    <div className="row p-2">
                    <div className="final-long border rounded-2 border-2 border-dark p-2 m-2">
                            <p>
                                <strong>
                            <div className="border-2 border-bottom border-dark"><h4>Additional abilities</h4></div><br/></strong>
                            <p className="item-final">{additionalAbilities}</p>
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