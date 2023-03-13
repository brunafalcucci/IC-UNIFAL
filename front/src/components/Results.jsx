import React from 'react'
import styled from 'styled-components'
import NavBar from './NavBar'

const Results = () => {
  return (
    <WrapResults> 
      <NavBar/> 
      <TotalDiv> 
         <WrapTitle> OVERALL RESULTS  </WrapTitle>

         <WrapImgeLevel> 
        
        <WrapperLevels> 
        <WrapLevel1> 
          <TitleLevel> Level 1 </TitleLevel>
          <WrapResult> 
          <WrapDiv> Operation / Work </WrapDiv>
          <WrapDiv> Activities </WrapDiv>
          </WrapResult>
        </WrapLevel1>  

        <WrapLevel2> 
        <TitleLevel> Level 2 </TitleLevel>
         <WrapResult> 
          <WrapDiv> Investments </WrapDiv>
          <WrapDiv> Energy Use </WrapDiv>
          <WrapDiv> Predictive </WrapDiv>
          <WrapDiv> Maintenance </WrapDiv>
          <WrapDiv> Preventive </WrapDiv>
          <WrapDiv> Governance </WrapDiv>
          <WrapDiv> Enviromental Risk  </WrapDiv>
          </WrapResult> 
        </WrapLevel2>  

        <WrapLevel3> 
        <TitleLevel> Level 3 </TitleLevel>
        <WrapResult> 
          <WrapDiv> Costs Management </WrapDiv>
          <WrapDiv> Industrial Management </WrapDiv>
          <WrapDiv> Environmental Quality </WrapDiv>
          </WrapResult> 
        </WrapLevel3>  


        <WrapLevel4> 
        <TitleLevel> Level 4 </TitleLevel>
        <WrapResult> 
          <WrapDiv> CRITICALITY INDEX </WrapDiv>
          </WrapResult> 
        </WrapLevel4>  
         
        <WrapLevel5> 
        <TitleLevel> LEGENDA </TitleLevel>
          <WrapLegenda>  
          <Legenda> CRITICAL </Legenda>
          <Legenda> POTENTIALLY CRITICAL </Legenda>
          <Legenda> ACCEPTABLE </Legenda>
          <Legenda> POTENTIALLY UNCRITICAL </Legenda>
          <Legenda> UNCRITICAL </Legenda>
          </WrapLegenda>  
          </WrapLevel5>  

        </WrapperLevels>

        </WrapImgeLevel>

         </TotalDiv>
    </WrapResults>
  )
}

const WrapResults = styled.div`
display: flex;
`

const TotalDiv = styled.div`
width: 100%;
`
const WrapTitle = styled.h1` 
  font-size: 40px;
  color: green !important;
  text-align: center;
`

const WrapLevel1 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
margin-top: 1rem;
`

const WrapLevel2 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
margin-top: 1rem;
`
const WrapLevel3 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
margin-top: 1rem;
`

const WrapLevel4 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
margin-top: 1rem;
`
const WrapLevel5 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
margin-top: 1rem;
display: flex;
flex-direction:column;
justify-content:center;
align-items: center;
`

const WrapLegenda = styled.div`
display: flex;
flex-wrap: wrap;
gap: 3rem;
padding-bottom:2rem;

`

const Legenda = styled.div`
position: relative;
&::after{
  content: '';
  position: absolute;
  left:-2rem;
  width:1.5rem;
  aspect-ratio:  1/1;
  background-color:red;
  border-radius:99999999vw;
}
&:nth-of-type(2){
  &::after{
    background-color: #FF9900;
  }
}
&:nth-of-type(3){
  &::after{
    background-color: #FAFF00;
  }
}
&:nth-of-type(4){
  &::after{
    background-color: #A4D713;
  }
}
&:nth-of-type(5){
  &::after{
    background-color: #068E0B;
  }
}
`

const TitleLevel = styled.h2`
text-align: center;
color: green;
`

const WrapDiv = styled.div`
  border: 1px solid grey;
  width: 15%;
  border-radius: 10px;
 
  
`
const WrapImgeLevel = styled.div`
display: flex; 
flex-direction: column;
`

const WrapperLevels = styled.div`
display: flex;
flex-direction: column;
align-items: center;

`

const WrapResult = styled.div`
display: flex;
justify-content: center;
gap: 2rem;
margin-bottom: 1rem;
`
export default Results