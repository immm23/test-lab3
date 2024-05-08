import dice from "./images/d20.png";
import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import Button from "@mui/material/Button";

export default function StartUpPage(props){
    return(
        <div className="start-up border border-dark border-2 rounded-2">
            <img src = {dice} width={"100px"} height={"100px"} className = "image-start-up rounded mx-auto d-block" 
                alt="dice"/>
            <h3>
                Are you ready to create your character?
            </h3>
            <Button onClick={() => {props.this(false); props.next(true);}} variant="outlined"><strong>Create new character</strong></Button>
        </div>
    );
}