import './App.css';
import {Route, Routes} from 'react-router-dom';
import FormCalculo from './components/FormCalculo';
import NavBar from './components/NavBar';
import FormEfficiency from './components/FormEfficiency';
function App() {
  return (
    <>

    <Routes> 
      <Route path="/" element={<NavBar />} /> 
      <Route path="/form" element={<FormCalculo />} /> 
      <Route path="/efficiency" element={<FormEfficiency />} />
    </Routes>
    </>
  );
}

export default App;
