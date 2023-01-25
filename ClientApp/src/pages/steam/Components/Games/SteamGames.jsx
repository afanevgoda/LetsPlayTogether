import React, {useCallback, useState} from 'react';
import Paper from '@mui/material/Paper';
import Grid from "@mui/material/Unstable_Grid2";
import Button from "@mui/material/Button";
import GamePoll from "../GamePoll/GamePoll";
import styles from './SteamGames.module.css';
import GamePanel from "../GamePanel/GamePanel";
import _ from 'lodash';
import {Collapse} from "@mui/material";

export function SteamGames({matchedGames, addPollAnswerForAGame, poll}) {

    const [collapsed, setCollapsed] = useState([]);

    const groupsByNumberOfOwningPlayers = _.groupBy(matchedGames, x => x.numberOfOwningPlayers);

    const getSectionName = (numberOfOwningPlayers) => {
        if (numberOfOwningPlayers === 0) return 'Nobody has these games... weird';
        if (numberOfOwningPlayers === 1) return 'Only one of the party have these'; else return `${numberOfOwningPlayers} teammates have these games`;
    };

    return (<>
        {_.orderBy(_.keys(groupsByNumberOfOwningPlayers), x => parseInt(x, 10), 'desc')
            .map(x => {
                return (<>
                    <h1>{getSectionName(parseInt(x, 10))}</h1>
                    <Grid container spacing={1}>
                        {groupsByNumberOfOwningPlayers[x].map(x => <Grid xs={12} md={6} lg={4} xl={3}>
                            <Paper key={x.appId} className={styles.gamePanel}>
                                <GamePanel gameInfo={x}/>
                                <GamePoll
                                    poll={poll?.votes ? poll?.votes.find(r => r.appId === x.appId) : undefined}
                                    onChange={(event) => addPollAnswerForAGame(x.appId, event.target.value)}/>
                            </Paper>
                        </Grid>)}
                    </Grid>
                </>)
            })}
    </>)
}