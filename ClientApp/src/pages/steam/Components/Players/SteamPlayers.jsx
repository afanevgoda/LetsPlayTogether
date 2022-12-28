import {getPlayersInfo, getMatchedGames} from "../../../../service/api";
import React, {Component, useState} from 'react';
import Grid from '@mui/material/Unstable_Grid2';
import Paper from '@mui/material/Paper';
import styles from './Players.module.css';
import Button from "@mui/material/Button";
import Input from "@mui/material/Input";
import {Divider, IconButton, InputBase} from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';

export function SteamPlayers({setMatchedGames}) {

    const [players, setPlayers] = useState([]);
    const [playersInfo, setPlayersInfo] = useState([]);

    const addTeammate = () => {
        setPlayers([...players, {tempId: players.length + 1}]);
    }

    const getPlayers = async () => {
        getPlayersInfo(players)
            .then(response => response.json())
            .then(result => {
                setPlayersInfo(result);
            });
    }

    const removePlayer = (playerTempId) => {
        setPlayers(players.filter(x => x.tempId !== playerTempId));
    };

    const onInputChange = (tempId, event) => {
        let updatedPlayers = players.map((x, i) => {
            if (x?.tempId === tempId) x.url = event.target.value;
            return x;
        })
        setPlayers(updatedPlayers)
    }

    const getGames = async () => {
        getMatchedGames(playersInfo)
            .then(response => response.json())
            .then(result => {
                setMatchedGames(result);
            });
    }

    const playerInputComp = (player) => {
        return (<Grid xs={2} id={player.tempId}>
            <Paper
                className={styles.playerPanel}
            >
                <Input
                    className={styles.playerInput}
                    onChange={(event) => onInputChange(player.tempId, event)}
                    disableUnderline={true}
                    placeholder="Profile URL"
                />
                <IconButton type="button" sx={{p: '10px'}} aria-label="search">
                    <CloseIcon color="primary" onClick={() => removePlayer(player.tempId)}/>
                </IconButton>
            </Paper>
        </Grid>)
    };

    const playerPanelComp = (nickname, avatarUrl) => {
        return (<Grid xs={3} id={nickname}>
            <Paper className={styles.playerInfoPanel}>
                <span className={styles.playerAvatar}>
                    <img src={avatarUrl} alt={"none"}></img>
                </span>
                <span className={styles.nickname}>{nickname}</span>
            </Paper>
        </Grid>)
    };

    return (<>
        <Paper className={styles.box}>
            <Grid container spacing={2}>
                {players.map(x => playerInputComp(x))}
                <Grid>
                    <Paper>
                        <Button
                            variant="outlined"
                            size="small"
                            onClick={addTeammate}
                            className={styles.addTeammateButton}
                        >Add teammate</Button>
                    </Paper>
                </Grid>
            </Grid>
        </Paper>
        <Button
            variant='primary'
            size="small"
            onClick={getPlayers}>
            Check players
        </Button>
        <Divider />
        <Grid container spacing={2}>
            {playersInfo.map(x => playerPanelComp(x.nickname, x.avatarUrl))}
        </Grid>
        <Button
            variant='primary'
            size="small"
            onClick={getGames}>
            Get Games!
        </Button>
    </>)
}