import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import React, { useContext, useEffect, useState } from "react";
import {apiURL} from "./APIURL";
import { ColorRing } from 'react-loader-spinner';
import { Context } from "./Context";

export default function SubRace(props){

    const[image, setImage] = useState();
    const[isLoading, setIsLoading] = useState(true);
    const context = useContext(Context);

    useEffect(() => {
        fetch(apiURL +'/images/' + props.imagePath  + '/get', {
            method: 'GET'
            })
            .then((response) => {
                return response.blob();
            })
            .then((data) => {
                setImage(URL.createObjectURL(data));
                setIsLoading(false);
            })
            .catch((error) => console.log(error));

    }, []);

    return(
        <div className="subrace border border-2 border-dark rounded-2 text-center">
            <label>
                {props.id == context.subraceId ? 
                <input type="radio" className="radio" name="subRace" onSelect={context.subraceId = props.id}
                checked/> 
                :
                <input type="radio" className="radio" name="subRace" onSelect={context.subraceId = props.id}/>
                }
                
                <div className="subrace-radio-component border rounded-1 border-dark">
                    <div className=" d-flex p-2 justify-content-center ">
                        {!isLoading &&
                            <img className="image" alt="Subrace" width="25%" height="25%"src={image}/>
                        }
                        {isLoading &&
                            <div className="d-flex justify-content-center">
                            <ColorRing
                            visible={true}
                            height="75%"
                            width="75%"
                            ariaLabel="blocks-loading"
                            wrapperStyle={{}}
                            wrapperClass="blocks-wrapper"
                            colors={['#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0']}
                            />
                            </div>
                        }
                        
                    </div>
                    <div className="border-bottom border-2 border-dark m-2">
                        <p><strong>{props.name}</strong></p>
                    </div>
                    <div className="border-bottom border-1 border-dark m-2">
                        <p>{props.description}</p>
                    </div>
                    <div className="border-bottom border-1 border-dark m-2">
                        <p>{props.specialAbility}</p>
                    </div>
                    <div className="border-bottom border-1 border-dark m-2">
                        <p>{props.raceBonus}</p>
                    </div>
                </div>

            </label>
        </div>
    );
}