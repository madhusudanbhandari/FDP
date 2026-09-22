import { BrowserRouter,Routes,Route } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Restaurant from "./pages/Restaurant";
import ProtectedRoute from "./components/ProtectedRoute";

function App(){
  return(
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login/>}/>
        <Route path="/login" element={<Login/>}/>
        <Route path="/register" element={<Register/>}/>
       
        <Route path="/restaurants" 
              element={
              <ProtectedRoute>
                  <Restaurant/>
              </ProtectedRoute>
              }/>
      </Routes>
    </BrowserRouter>
  )
}

export default App;