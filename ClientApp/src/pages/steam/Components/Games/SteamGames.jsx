import React, {useState} from 'react';
import Paper from '@mui/material/Paper';
import Grid from "@mui/material/Unstable_Grid2";
import Button from "@mui/material/Button";
import GamePoll from "../GamePoll/GamePoll";
import styles from './SteamGames.module.css';
import GamePanel from "../GamePanel/GamePanel";

export function SteamGames({matchedGames, addPollAnswerForAGame, poll}) {

    return (<>
        <Grid container spacing={4}>
            {matchedGames.map(x => <Paper key={x.appId} className={styles.gamePanel}>
                <GamePanel gameInfo={x}/>
                <GamePoll
                    poll={poll?.votes ? poll?.votes.find(r => r.appId === x.appId) : undefined}
                    onChange={(event) => addPollAnswerForAGame(x.appId, event.target.value)}/>
            </Paper>)}
        </Grid>
    </>)
}