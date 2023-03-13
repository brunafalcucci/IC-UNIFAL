import React, { useState } from 'react';
import styled from 'styled-components';
import NavBar from './NavBar';
import axios from 'axios';



function Formulario() {
  const [availabilityOfRepairPersonnel, setAvailabilityOfRepairPersonnel] = useState('');
  const [operationSensitivity, setOperationSensitivity] = useState('');
  const [workLoad, setWorkLoad] = useState('');
  const [meanTimeBetweenFailures, setMeanTimeBetweenFailures] = useState('');
  const [averageRepairTime, setAverageRepairTime] = useState('');
  const [availabilityOfRequiredParts, setAvailabilityOfRequiredParts] = useState('');
  const [energyGeneration, setEnergygeneration] = useState('');
  const [technology, setTechnology] = useState('');
  const [process, setProcess] = useState('');
  const [technologyDataCollection, setTechnologyDataCollection] = useState('');
  const [instrumentation, setInstrumentation] = useState('');
  const [predictiveOperation, setPredictiveOperation] = useState('');
  const [skillLevels, setSkillLevels] = useState('');
  const [managementStrategy, setManagementStrategy] = useState('');
  const [toxity, setToxity] = useState('');
  const [solubility, setSolubility] = useState('');
  const [noRenewable, setNoRenewable] = useState('');
  const [renewable, setRenewable] = useState('');
  const [corrective, setCorrective] = useState('');
  const [electrcityExpensives, setElectrcityExpensives] = useState('');
  const [industrySector, setIndustrySector] = useState('');
  const [industryName, setIndustryName] = useState('');


  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post(`/api/CriticalityIndex/InsertCriticalityIndex, 
      Availability of repair personnel: ${availabilityOfRepairPersonnel}\n
      Operation Sensitivity: ${operationSensitivity}\n
      Work Load: ${workLoad}\n
      Mean time between failures: ${meanTimeBetweenFailures}\n
      Average repair time: ${averageRepairTime}\n
      Availability of required parts: ${availabilityOfRequiredParts} \n
      Energy generation: ${energyGeneration} \n
      Technology: ${technology} \n
      Process: ${process} \n
      Technology Data Collection: ${technologyDataCollection} \n
      Instrumentation: ${instrumentation} \n
      Predictive Operation: ${predictiveOperation} \n
      Skill Levels: ${skillLevels} \n
      Management Strategy: ${managementStrategy} \n
      Toxity: ${toxity} \n
      Solubility: ${solubility} \n
      No-Renewable: ${noRenewable} \n
      Renewable: ${renewable} \n
      Corrective: ${corrective} \n
      Electrcity Expensives: ${electrcityExpensives} \n
      Industry Sector: ${industrySector} \n
      Industry Name: ${industryName} \n

    `);
  };

  return (
   <Main>
        <NavBar> </NavBar>
        <FormWrapper>
        <TitleOut> CRITICALITY INDEX FORM </TitleOut>
    <form onSubmit={handleSubmit}>
        <WrapFormNav> 
         <WrapTotalForm>
         <TitleForm> Industry Data  </TitleForm>
         <WrapLabels>
         <label>
        Industry Sector: 
        <input type="text" value={industrySector} onChange={(event) => setIndustrySector(event.target.value)} />
      </label>
      <label>
        Industry Name: 
        <input type="text" value={industryName} onChange={(event) => setIndustryName(event.target.value)} />
      </label>
         </WrapLabels>
        <TitleForm> Environmental Quality </TitleForm>
        <WrapLabels>

      <label>
        Toxity: 
        <input type="text" value={toxity} onChange={(event) => setToxity(event.target.value)} />
      </label>
      
      <label>
        Solubility:
        <input type="text" value={solubility} onChange={(event) => setSolubility(event.target.value)} />
      </label>
      
      <label>
        No-Renewable:
        <input type="text" value={noRenewable} onChange={(event) => setNoRenewable(event.target.value)} />
      </label>
      
      <label>
        Renewable:
        <input type="text" value={renewable} onChange={(event) => setRenewable(event.target.value)} />
      </label>
   
        </WrapLabels>
        <TitleForm> Costs Management </TitleForm>
        <WrapLabels>

      <label>
        Energy Generation:
        <input type="text" value={energyGeneration} onChange={(event) => setEnergygeneration(event.target.value)} />
      </label>
      
      <label>
        Technology:
        <input type="text" value={technology} onChange={(event) => setTechnology(event.target.value)} />
      </label>
      
      <label>
        Process:
        <input type="text" value={process} onChange={(event) => setProcess(event.target.value)} />
      </label>

      <label>
        Electrcity Expensives:
        <input type="text" value={electrcityExpensives} onChange={(event) => setElectrcityExpensives(event.target.value)} />
      </label>

        </WrapLabels>
        <TitleForm> Industrial Management </TitleForm>
        <WrapLabels>

      <label>
        Technology Data Collection:
        <input type="text" value={technologyDataCollection} onChange={(event) => setTechnologyDataCollection(event.target.value)} />
      </label>

      <label>
        Instrumentation:
        <input type="text" value={instrumentation} onChange={(event) => setInstrumentation(event.target.value)} />
      </label>
      
      <label>
        Predictive Operation:
        <input type="text" value={predictiveOperation} onChange={(event) => setPredictiveOperation(event.target.value)} />
      </label>

      <label>
        Availability of repair personnel:
        <input type="text" value={availabilityOfRepairPersonnel} onChange={(event) => setAvailabilityOfRepairPersonnel(event.target.value)} />
      </label>
      
      <label>
        Operation Sensitivity:
        <input type="text" value={operationSensitivity} onChange={(event) => setOperationSensitivity(event.target.value)} />
      </label>

      <label>
        Mean time between failures:
        <input type="text" value={meanTimeBetweenFailures} onChange={(event) => setMeanTimeBetweenFailures(event.target.value)} />
      </label>

      <label>
        Work Load:
        <input type="text" value={workLoad} onChange={(event) => setWorkLoad(event.target.value)} />
      </label>

      <label>
        Average repair time:
        <input type="text" value={averageRepairTime} onChange={(event) => setAverageRepairTime(event.target.value)} />
      </label>

      <label>
        Availability of required parts:
        <input type="text" value={availabilityOfRequiredParts} onChange={(event) => setAvailabilityOfRequiredParts(event.target.value)} />
      </label>

      <label>
        Corrective:
        <input type="text" value={corrective} onChange={(event) => setCorrective(event.target.value)} />
      </label>
      
      <label>
        Skill Levels:
        <input type="text" value={skillLevels} onChange={(event) => setSkillLevels(event.target.value)} />
      </label>
      
      <label>
        Management Strategy:
        <input type="text" value={managementStrategy} onChange={(event) => setManagementStrategy(event.target.value)} />
      </label>

        </WrapLabels>
        <button type="submit">Submit</button>
        </WrapTotalForm> 
        </WrapFormNav>
    </form>
        </FormWrapper>
    </Main>
  );
}

const Main = styled.div`
display :flex;
`
 const TitleForm = styled.h1`
    color: green;
    text-align: center;
 `

 const WrapLabels = styled.div` 
 
 text-align: center;
 height: 100%;
 width: 0 auto;
 display: grid;
 grid-template-columns: repeat(2,1fr);
 @media (max-width: 700px){
    grid-template-columns: 1fr;
 }
 `

 const WrapTotalForm = styled.div`
 display: flex;
flex-direction: column;
align-items: center;
border: 2px solid grey;
border-radius: 10px;
margin-top: 1rem;
margin-bottom: 1rem;

label{
    font-size: 18px;
    display: grid;
    text-align:start;
    padding: 0.5rem 2rem ;
    
}
input{
    font-size: 20px;
    border-radius: 10px;
    box-shadow: 0px 0px 1px 1px #eedddd; 
    &:focus{
        outline: 2.5px solid #1ac24c;
    }
     
}

button{
    margin: 1rem;
    border: 2px solid green;
    border-radius: 15px;
    width: 30%;
    height: 60px;
    font-size: 20px;
    &:hover{
        background-color: green !important;
        color: white; 
    }
}
 `

 const WrapFormNav = styled.div`
 display: flex;
 `
 const FormWrapper = styled.div`
        width: 100%;
        display: grid;
        place-content:center;
    `

  const TitleOut  = styled.h1`
  font-size: 40px;
  color: green !important;
 text-align: center;
  `

export default Formulario;
