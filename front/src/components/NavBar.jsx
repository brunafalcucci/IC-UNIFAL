import React from 'react'
import styled from 'styled-components'
import logo from '../img/LOGO-IC.png'
import { Link } from 'react-router-dom'


const NavBar = () => {
  return (
    <WrapSide>  
      <img src={logo} alt="" />
      <WrapButtons> 
      <Link to='/form'> <button> CRITICALITY INDEX </button> </Link>
      <Link to='/efficiency'> <button> EFFICIENCY ENERGETIC </button> </Link>
      <Link to='/results'> <button> RESULTS TEST </button> </Link>
      <Link to='/chemical'> <button> CHEMICAL TREATMENT </button> </Link>
      <button> ALGUMA COISA </button>
      <button> ALGUMA COISA </button>
      </WrapButtons>
    </WrapSide>
  )
}


const WrapSide = styled.div`
width: clamp(5rem, 15vw, 15rem);
min-height: 100vh;
background-color: #D4F8B9; 
img {
  width: 90%;
}
`
const WrapButtons = styled.div`
display: flex;
flex-direction: column;
margin-left: 10%;
width: 75%;
button { 
margin: 10%;
border: 2px solid #525250;
border-radius: 10px;
font-size: 15px; 
background-color: #D4F8B9;
box-shadow: 2px 2px 2px 1px rgba(0, 0, 0, 0.2);
@media (max-width: 40em){
font-size: 8px;
}

}

`
export default NavBar