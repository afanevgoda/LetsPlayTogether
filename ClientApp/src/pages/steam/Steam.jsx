import React, {Component, useState} from 'react';
import Button from '@mui/material/Button';
import Input from '@mui/material/Input';

export function Steam() {
    const [players, setPlayers] = useState([]);
    const [matchedGames, setMatchedGames] = useState([]);

    const onInputChange = (event) => {
        setPlayers(event.target.value)
    }

    const callApi = async () => {
        // let params = players.map(x => `playerUrls=${x}`);
        const response = await fetch(`https://localhost:7220/Games?${players}`, {
            method: 'GET',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {'Content-Type': 'application/json'},
            redirect: 'follow',
            referrerPolicy: 'no-referrer'
        })
            .then(response => response.json())
            .then(result => {
                console.log(result);
                setMatchedGames(result);
            });
    }

    return (<>
        <div>
            <Button variant="contained">Add teammate</Button>
            <Input onChange={onInputChange}></Input>
            <Button variant="contained" onClick={callApi}>Submit</Button>
            {matchedGames.map(x => 
                <>
                    <div>{x?.name}</div>
                    <img  src={x?.headerImage}/>
                </>
            )}
        </div>
    </>);
}
