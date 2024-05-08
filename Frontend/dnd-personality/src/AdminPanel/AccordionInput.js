import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

export default function AccordionInput(props){
    return(
        <Accordion sx ={{border:2, background: 'rgb(219, 196, 105)'}}>
            <AccordionSummary
            sx = {{borderBottom:2}}
            expandIcon={<ExpandMoreIcon />}
            >
            <div sx ={{background: 'black'}}>{props.name}</div>
            </AccordionSummary>
            <AccordionDetails className='accordeon-content'>
                    {props.content}
            </AccordionDetails>
        </Accordion>
    );
}