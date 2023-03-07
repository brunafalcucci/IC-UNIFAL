import './App.css';
import {Route, Routes} from 'react-router-dom';
import FormCalculo from './components/FormCalculo';
import FormEfficiency from './components/FormEfficiency';
import Inicial from './components/Inicial';
import Results from './components/Results';


function App() {
  return (
    <>

    <Routes> 
      <Route path="/" element={<Inicial />} /> 
      <Route path="/form" element={<FormCalculo />} /> 
      <Route path="/efficiency" element={<FormEfficiency />} />
      <Route path="/results" element={<Results />} /> 
    </Routes>
    </>
  );
}

export default App;
