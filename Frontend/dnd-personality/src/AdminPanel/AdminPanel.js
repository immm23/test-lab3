import React, { useState} from "react";
import Tab from "@mui/material/Tab";
import Tabs from "@mui/material/Tabs";
import Typography from '@mui/material/Typography';
import AccordionInput from "./AccordionInput";
import "../styles.css";
import "bootstrap/dist/css/bootstrap.css";
import LanguageAdmin from "./LanguageAdmin";
import WeaponAdmin from "./WeaponAdmin";
import ArmorAdmin from "./ArmorAdmin";
import RaceAdmin from "./RaceAdmin";
import SubraceAdmin from "./SubraceAdmin";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@mui/material";
import {useNavigate} from 'react-router-dom';
import { ColorRing } from "react-loader-spinner";
import SkillAdmin from "./SkillAdmin";
import ToolAdmin from "./ToolAdmin";
import ClassAdmin from "./ClassAdmin";
import ItemAdmin from "./ItemAdmin";


function TabPanel(props) {
    const { children, value, index, ...other } = props;
    return (
      <div
        role="tabpanel"
        hidden={value !== index}
        id={`simple-tabpanel-${index}`}
        aria-labelledby={`simple-tab-${index}`}
        {...other}
      >
        {value === index && (
            <Typography>{children}</Typography>
        )}
      </div>
    );
  }

export default function Admin(){

    const [selectedValue, setValue] = useState(0);
    const { loginWithRedirect } = useAuth0();
    const { logout } = useAuth0();
    const { user, isAuthenticated, isLoading} = useAuth0();
    const navigate = useNavigate();

    const navigateHome = () => {
      navigate('/');
    };



    const tabChange = (event, newValue) => {
        setValue(newValue);
    };

    if(isLoading){
      return(<div className="start-up">
        <ColorRing
          visible={true}
          height="200"
          width="200"
          ariaLabel="blocks-loading"
          wrapperStyle={{}}
          wrapperClass="blocks-wrapper"
          colors={['#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0', '#AA80E0']}
          />
      </div>);
    }
    return(
      <div>
      {isAuthenticated &&
        <div>
        <div className="admin">
        <div className="admin-header border-bottom border-2 border-dark flex-right d-flex justify-content-end p-1">
          <Button onClick={navigateHome} variant="outlined" className="m-1">Main page</Button>
          <p className="font-weight-bold m-2">{user.name}</p>
          <Button onClick={logout} variant="outlined" className="m-1">Log out</Button>
        </div>
            <Tabs value={selectedValue} onChange={tabChange} centered>
                <Tab label="Items" />
                <Tab label="Race" />
                <Tab label="Subrace" />
                <Tab label="Class" />

            </Tabs>
              <TabPanel value={selectedValue} index={0}>
              <AccordionInput name="Language" content ={<LanguageAdmin/>}/>
              <AccordionInput name="Skill" content ={<SkillAdmin/>}/>
              <AccordionInput name="Tool" content ={<ToolAdmin/>}/>
              <AccordionInput name="Item" content ={<ItemAdmin/>}/>
              <AccordionInput name="Weapon" content ={<WeaponAdmin/>}/>
              <AccordionInput name="Armor" content ={<ArmorAdmin/>}/>
            </TabPanel>
            <TabPanel value={selectedValue} index={1}>
                <AccordionInput name="Race" content ={<RaceAdmin/>}/>
            </TabPanel>
            <TabPanel value={selectedValue} index={2}>
            <AccordionInput name="Subrace" content = {<SubraceAdmin/>}/>
            </TabPanel>
            <TabPanel value={selectedValue} index={3}>
            <AccordionInput name="Class" content = {<ClassAdmin/>}/>
            </TabPanel>
          </div>
        </div>
      }
      {
        !isAuthenticated && 
        <div className="start-up ">
        <Button size="large" onClick={loginWithRedirect} variant="outlined">Log in</Button>
        </div> 
      }
      </div>
    );
}