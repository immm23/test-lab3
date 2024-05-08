 import AdminPanel from "./AdminPanel/AdminPanel";
 import Main from "./Main";
 import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
 import { CreationSettings } from "./Context";
 import { Context } from "./Context";

 import React, { useEffect } from "react";

 export default function App() {

  useEffect(() => {
    document.title = 'DND personality';
  }, []);
  console.log(process.env.REACT_APP_API_URL);
  return (
    <div className="App">
        <Router>
          <Routes>
            <Route path="/admin">
              <AdminPanel/>
            </Route>
            <Route path="/">
              <Context.Provider value={CreationSettings}>
                <Main/>
              </Context.Provider>
            </Route>
          </Routes>
        </Router>
    </div>

  );
}
