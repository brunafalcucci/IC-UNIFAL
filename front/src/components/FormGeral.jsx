import React, { useState } from "react";
import styled from "styled-components";
import NavBar from "./NavBar";

export function FormResiduosQuimicosEnergia() {
  const [destiledWater, setDestiledWater] = useState("");
  const [oil, setOil] = useState("");
  const [ink, setInk] = useState("");
  const [reutilization, setReutilization] = useState("");
  const [solution, setSolution] = useState("");
  const [recycling, setRecycling] = useState("");
  const [remains, setRemains] = useState("");
  const [contamination, setContamination] = useState("");
  const [sand, setSand] = useState("");
  const [recovery, setRecovery] = useState("");
  const [flocculation, setFlocculation] = useState("");
  const [slimeRemoval, setSlimeRemoval] = useState("");
  const [heatGeneration, setHeatGeneration] = useState("");
  const [operation, setOperation] = useState("");
  const [trashDeposit, setTrashDeposit] = useState("");
  const [water, setWater] = useState("");
  const [employees, setEmployees] = useState("");
  const [stopper, setStopper] = useState("");
  

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log(destiledWater);
    console.log(oil);
    console.log(ink);
    console.log(reutilization);
    console.log(solution);
    console.log(recycling);
    console.log(remains);
    console.log(contamination);
    console.log(sand);
    console.log(recovery);
    console.log(flocculation);
    console.log(slimeRemoval);
    console.log(heatGeneration);
    console.log(operation);
    console.log(trashDeposit);
    console.log(water);
    console.log(employees);
    console.log(stopper);
  };

  return (
    <Main>
      <NavBar> </NavBar>
      <FormWrapper>
        <form onSubmit={handleSubmit}>
          <WrapFormNav>
            <WrapTotalForm>
              <TitleForm> Chemical residues energy </TitleForm>
              <WrapLabels>
                <label>
                  Destiled Water:
                  <select
                    value={destiledWater}
                    onChange={(event) => setDestiledWater(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5131">Recycling</option>
                    <option value="3,5132">Reutilization</option>
                  </select>
                </label>
                <label>
                  Oil:
                  <select
                    value={oil}
                    onChange={(event) => setOil(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5111">Hydraulic</option>
                    <option value="3,5112">Reutilization</option>
                    <option value="3,5113">Selling</option>
                  </select>
                </label>
                <label>
                  Ink:
                  <select
                    value={ink}
                    onChange={(event) => setInk(event.target.value)}
                  >
                    <option value="3,5131">Recycling</option>
                  </select>
                </label>
                <label>
                Reutilization:
                  <select
                    value={reutilization}
                    onChange={(event) => setReutilization(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5145">Acid Bath</option>
                    <option value="3,5146">Metal Fluid</option>
                  </select>
                </label>
                <label>
                Solution:
                  <select
                    value={solution}
                    onChange={(event) => setSolution(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5142">Cleaning</option>
                    <option value="3,5143">Manufacturer</option>
                    <option value="3,5144">Recycling</option>
                  </select>
                </label>
                <label>
                Recycling:
                  <select
                    value={recycling}
                    onChange={(event) => setRecycling(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5126">Non-ferrous Powder</option>
                    <option value="3,5127">Paper</option>
                    <option value="3,5128">Rubber</option>
                  </select>
                </label>
                <label>
                Remains:
                  <select
                    value={remains}
                    onChange={(event) => setRemains(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5211">Glass Scraps</option>
                    <option value="3,5212">Plastic Parts</option>
                    <option value="3,5213">Printed Papper</option>
                  </select>
                </label>
                <label>
                Contamination:
                  <select
                    value={contamination}
                    onChange={(event) => setContamination(event.target.value)}
                  >
                    <option value="3,5215">Parts</option>
                  </select>
                </label>
                <label>
                Sand:
                  <select
                    value={sand}
                    onChange={(event) => setSand(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5241">Recycling</option>
                    <option value="3,5246">Use</option>
                  </select>
                </label>
                <label>
                Recovey:
                  <select
                    value={recovery}
                    onChange={(event) => setRecovery(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,5242">Metals</option>
                    <option value="3,5243">Film</option>
                    <option value="3,5244">Foundry Sand</option>
                    <option value="3,5245">Waste</option>
                  </select>
                </label>
                <label>
                flocculation:
                  <select
                    value={flocculation}
                    onChange={(event) => setFlocculation(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,6111">Sludge</option>
                    <option value="3,6114">Residual Waters</option>
                  </select>
                </label>
                <label>
                Slime Removal:
                  <select
                    value={slimeRemoval}
                    onChange={(event) => setSlimeRemoval(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,6112">Greenhouse Filter</option>
                    <option value="3,6113">Tanks</option>
                  </select>
                </label>
                <label>
                Heat Reneration:
                  <select
                    value={heatGeneration}
                    onChange={(event) => setHeatGeneration(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,6122">Incinerator</option>
                    <option value="3,6123">Wood</option>
                    <option value="3,6124">Waste Oil</option>
                  </select>
                </label>
                <label>
                Operation:
                  <select
                    value={operation}
                    onChange={(event) => setOperation(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,6121">Paper Burning</option>
                    <option value="3,6126">Waste Gases</option>
                    <option value="3,6125">Sale of Waste</option>
                  </select>
                </label>
                <label>
                Trash Deposit:
                  <select
                    value={trashDeposit}
                    onChange={(event) => setTrashDeposit(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,6192">Cheap Waste</option>
                    <option value="3,6193">Equipment</option>
                    <option value="3,6191">Manufacturer</option>
                    <option value="3,6194">Hydraulic Oil</option>
                  </select>
                </label>
                <label>
                Water:
                  <select
                    value={water}
                    onChange={(event) => setWater(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,8111">Separator</option>
                    <option value="3,8112">Deionized Water</option>
                  </select>
                </label>
                <label>
                Stopper:
                  <select
                    value={stopper}
                    onChange={(event) => setStopper(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,8122">Adjustable Lids</option>
                    <option value="3,8124">Material Tanks</option>
                  </select>
                </label>
                <label>
                Employees:
                  <select
                    value={employees}
                    onChange={(event) => setEmployees(event.target.value)}
                  >
                    <option value=""></option>
                    <option value="3,8113">Solvents</option>
                    <option value="3,8115">Parts Wash</option>
                  </select>
                </label>
                {/* <select
                    type="text"
                    value={destiledWater}
                    onChange={(event) =>
                      setDestiledWater(event.target.value)
                    }
                  /> */}
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
  display: flex;
`;
const TitleForm = styled.h1`
  color: green;
`;

const WrapLabels = styled.div`
  text-align: center;
  height: 100%;
  width: 0 auto;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  @media (max-width: 700px) {
    grid-template-columns: 1fr;
  }
`;

const WrapTotalForm = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 2px solid grey;
  border-radius: 10px;
  margin-top: 1rem;
  margin-bottom: 1rem;

  label {
    font-size: 18px;
    display: grid;
    text-align: start;
    padding: 0.5rem 2rem;
  }
  input {
    font-size: 20px;
    border-radius: 10px;
    box-shadow: 0px 0px 1px 1px #eedddd;
    &:focus {
      border: 4px solid #1ac24c;
    }
  }

  button {
    margin: 1rem;
    border: 2px solid green;
    border-radius: 10px;
    width: 25%;
    height: 40px;
    &:hover {
      background-color: green !important;
      color: white;
    }
  }
`;

const WrapFormNav = styled.div`
  display: flex;
`;
const FormWrapper = styled.div`
  width: 100%;
  display: grid;
  place-content: center;
`;
