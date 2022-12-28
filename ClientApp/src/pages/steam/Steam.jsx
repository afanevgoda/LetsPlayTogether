import React, {useState} from 'react';
import {SteamPlayers} from "./Components/Players/SteamPlayers";
import {SteamGames} from "./Components/Games/SteamGames";

export function Steam() {
    const [matchedGames, setMatchedGames] = useState([]);
    
    return (<>
        <div>
            <SteamPlayers setMatchedGames={setMatchedGames}/>
            <SteamGames matchedGames={matchedGames} />
        </div>
    </>);
}
