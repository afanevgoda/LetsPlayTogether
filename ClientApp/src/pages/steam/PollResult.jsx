import React, {useEffect, useState} from 'react';
import {getGames, getPoll} from '../../service/api';
import PollGameResult from "./Components/PollGameResult/PollGameResult";
import Grid from "@mui/material/Unstable_Grid2";

export function PollResult() {

    const [poll, setPoll] = useState({});
    const [games, setGames] = useState([]);

    useEffect(() => {
        const params = new URLSearchParams(window.location.search)

        if (!params.has('pollId')) return;

        getPoll(params.get('pollId'))
            .then(x => x.json())
            .then(async x => {
                setPoll(x);
                await initGames(x.gameIds)
            });
    }, [])

    const initGames = async (gamesIds) => {
        getGames(gamesIds)
            .then(response => response.json())
            .then(result => {
                setGames(result);
            });
    }

    // todo: move to helpers VVV 
    function getShareLink() {
        // if (window.location.search.includes('poll')) return window.location.href;

        if (poll?.id) return `${window.location.origin}/steam?poll=${poll?.id}`;
    }
    
    return (<>
        <div>
            <span>Share this poll: {getShareLink()}</span>
            <Grid container>
                {games.map(x => <Grid>
                    <PollGameResult gameInfo={x} votes={poll?.results?.find(p => p.appId === x.appId)}/>
                </Grid>)}
            </Grid>
        </div>
    </>);
}
