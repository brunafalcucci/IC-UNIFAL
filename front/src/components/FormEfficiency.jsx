import React, { useState } from 'react';
import styled from 'styled-components';
import NavBar from './NavBar';
import axios from 'axios';



function Formulario() {
  const [industrySector, setIndustrySector] = useState('');
  const [industryName, setIndustryName] = useState('');


  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post(`/api, 
      Industry Sector: ${industrySector} \n
      Industry Name: ${industryName} \n

    `);
  };

  return (
   <Main>
        <NavBar> </NavBar>
        <FormWrapper>
        <TitleOut> EFFICIENCY ENERGETIC FORM </TitleOut>
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
