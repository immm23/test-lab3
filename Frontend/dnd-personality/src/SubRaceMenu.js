import {Context} from "./Context";
import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import StepButton from "./StepButton";
import SubRace from "./SubRace";
import React, { useContext, useEffect, useState } from "react";
import {apiURL} from "./APIURL";
import { ColorRing} from 'react-loader-spinner';

export default function SubRaceMenu(props){
    const context = useContext(Context);

    const [subraces, setSubraces] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        fetch(apiURL +'/subraces/all', {
            method: 'GET'
        })
        .then((response) =>{return response.json(); })
        .then((data) => {
            data = data.filter((subrace) => subrace.raceId == context.raceId)
            if(data.length == 0){
                setIsLoading(false);
                props.this(false);
                props.next(true)
            }
            else{
                setSubraces(data)
                setIsLoading(false);
            }
        })
        .catch((error) => console.log(error));
        
    }, []);

       return(
            <div className="race-menu">
                {!isLoading &&
                <div>
                <div className="race-menu-top  d-flex justify-content-between">
                        <StepButton onClick={()=> {props.this(false); props.previous(true);}} text="Back"/>
                        <h5>Subrace</h5>
                        <StepButton disabled={isLoading} text="Next" onClick={()=> {props.this(false); props.next(true);}}/>
                </div>
                <div className="subRaces d-flex flex-wrap justify-content-center align-items-center">
                   {subraces.map(subrace => (
                        <SubRace
                        id={subrace.id}
                        name={subrace.name} 
                        description={subrace.description}
                        specialAbility={subrace.specialAbility}
                        raceBonus={subrace.raceBonus.ability.name + " + "
                            + subrace.raceBonus.amount}
                        imagePath={subrace.imagePath}
                        key={subrace.id}/>

                    ))}
                </div>
                </div>
                }

                {isLoading &&
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