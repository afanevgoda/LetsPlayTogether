import React, {useState} from 'react';
import Paper from '@mui/material/Paper';
import Grid from "@mui/material/Unstable_Grid2";
import Button from "@mui/material/Button";
import GamePoll from "../GamePoll/GamePoll";
import styles from './SteamGames.module.css';

export function SteamGames({matchedGames}) {

    return (<>
        {matchedGames.map(x => <>
            <Paper 
                className={styles.gamePanel}
            >
                <Grid container spacing={4}>
                    <Paper 
                        className={styles.gameInfoPanel} 
                        lg={2} 
                        elevation={3}
                    >
                        <div>{x?.name}</div>
                        <img
                            className={styles.gameImage}
                            src={x?.headerImage}
                        />
                        <div>
                            <Button variant='primary'>Shop</Button>
                        </div>
                    </Paper>
                    <GamePoll />
                </Grid>
            </Paper>
            {/*<div>{x?.name}</div>*/}
        </>)}
    </>)
}