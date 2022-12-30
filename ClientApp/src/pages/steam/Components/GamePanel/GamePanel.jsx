import React from 'react';
import Paper from "@mui/material/Paper";
import styles from "../Games/SteamGames.module.css";

export default function GamePanel({gameInfo, extraComp}) {

    return (
        <Paper
            className={styles.gameInfoPanel}
            elevation={3}
        >
            <div
                className={styles.gameName}
            >{gameInfo?.name}</div>
            <img
                className={styles.gameImage}
                src={gameInfo?.headerImage}
            />
                {extraComp}
            {/*<div>*/}
            {/*    <Button variant='primary'>Shop</Button>*/}
            {/*</div>*/}
        </Paper>
    );
}