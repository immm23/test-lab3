import "./styles.css";
import "bootstrap/dist/css/bootstrap.css";
import Button from "@mui/material/Button";

export default function StepButton(props){
    return(
        <div>
            <Button disabled={props.disabled} onClick={() => {props.onClick()}} variant="outlined"><strong>{props.text}</strong></Button>
        </div>
    );
};