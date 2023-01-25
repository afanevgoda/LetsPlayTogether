import React from 'react';
import Paper from "@mui/material/Paper";
import styles from "../Games/SteamGames.module.css";

export default function GamePanel({gameInfo, extraComp}) {
    
    const getPlayerWhoDontHaveGamesLabel = (playersNicknames) => {
        
        if(playersNicknames === undefined)
            return;
        
        if(playersNicknames.length === 0 )
            return 'Everyone has this game!';
        if(playersNicknames.length === 1)
            return `${playersNicknames[0]} doesn't have this game`
        else if(playersNicknames.length > 1)
            return  `${playersNicknames.join(', ')} don't have this game`
    }
    
    const getPlayerNicknamesLabelColor = (playersNicknames) => {
        if(playersNicknames === undefined)
            return;

        if(playersNicknames.length === 0 )
            return styles.haveLabel;
        else
            return styles.dontHaveLabel;
    }
    
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
            <div className={styles.divider}></div>
            {/*<div className={getPlayerNicknamesLabelColor(gameInfo.playersThatDontHaveGame)}>*/}
            {/*    {getPlayerWhoDontHaveGamesLabel(gameInfo.playersThatDontHaveGame)}*/}
            {/*</div>*/}

            {extraComp}
            {/*<div>*/}
            {/*    <Button variant='primary'>Shop</Button>*/}
            {/*</div>*/}
        </Paper>
    );
}