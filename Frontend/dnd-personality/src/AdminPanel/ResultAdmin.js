import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";

export default function ResultAdmin(props){
    return(
        <div>
            {props.result ? <p className="text-success"><strong>Success</strong></p>: 
                <p className="text-danger"><strong>Error</strong></p>}
            {!props.result ? <p className="text-secondary">{props.errorDescription}</p> : null}
        </div>
    );
}