import React from 'react'
import styled from 'styled-components'
import NavBar from './NavBar'
import legenda from '../img/legenda.png'

const Results = () => {
  return (
    <WrapResults> 
      <NavBar/> 
      <TotalDiv> 
         <WrapTitle> OVERALL RESULTS  </WrapTitle>

         <TesteJoao> 
        
        
        <WrapLevel1> 
          <TitleLevel> Level 1 </TitleLevel>
          <WrapDiv> Operation / Work </WrapDiv>
          <WrapDiv> Activities </WrapDiv>
        </WrapLevel1>  

        <WrapLevel2> 
        <TitleLevel> Level 2 </TitleLevel>
          <WrapDiv> Investments </WrapDiv>
          <WrapDiv> Energy Use </WrapDiv>
          <WrapDiv> Predictive </WrapDiv>
          <WrapDiv> Maintenance </WrapDiv>
          <WrapDiv> Preventive </WrapDiv>
          <WrapDiv> Governance </WrapDiv>
          <WrapDiv> Enviromental Risk  </WrapDiv>
        </WrapLevel2>  

        <WrapLevel3> 
        <TitleLevel> Level 3 </TitleLevel>
          <WrapDiv> Costs Management </WrapDiv>
          <WrapDiv> Industrial Management </WrapDiv>
          <WrapDiv> Environmental Quality </WrapDiv>
        </WrapLevel3>  

        <WrapLevel4> 
        <TitleLevel> Level 4 </TitleLevel>
          <WrapDiv> CRITICALITY INDEX </WrapDiv>
        </WrapLevel4>  

         <img src={legenda} alt="" />
        </TesteJoao>

         </TotalDiv>
    </WrapResults>
  )
}

const WrapResults = styled.div`
display: flex;
`

const TotalDiv = styled.div`
width: 100%;
height: 100vh;
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
`

const WrapLevel2 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
`
const WrapLevel3 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
`

const WrapLevel4 = styled.div`
border: 0;
box-shadow: 0px 0px 5px 1px #DDD;
width: 60%;
`
const TitleLevel = styled.h2`
`

const WrapDiv = styled.div`
  border: 1px solid grey;
  width: 10%;
  border-radius: 10px;
`
const TesteJoao = styled.div`
display: flex; 
flex-direction: column;
img{
  width: 250px;
  margin-left: auto;
  @media (max-width: 40em){
     width: auto;
  }
}
`
export default Results