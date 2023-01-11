import {getPlayersInfo, getMatchedGames, createPoll} from "../../../../service/api";
import React, {Component, useState} from 'react';
import Grid from '@mui/material/Unstable_Grid2';
import Paper from '@mui/material/Paper';
import styles from './Players.module.css';
import Button from "@mui/material/Button";
import Input from "@mui/material/Input";
import {Divider, IconButton} from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';

export function SteamPlayers({setMatchedGames, poll, setPoll, isShared}) {

    const [players, setPlayers] = useState([]);
    const [playersInfo, setPlayersInfo] = useState([]);
    // for faster update from api itself
    const [pollId, setPollId] = useState(poll?.id);

    const addTeammate = () => {
        setPlayers([...players, {tempId: players.length + 1}]);
    }

    const getPlayers = async () => {
        getPlayersInfo(players)
            .then(response => response.json())
            .then(async result => {
                setPlayersInfo(result);
                await getGames(result);
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

    const getGames = async (info) => {
        getMatchedGames(info)
            .then(response => response.json())
            .then(result => {
                setMatchedGames(result);
                createPoll(players, result)
                    .then(x => x.text())
                    .then(x => {
                        setPollId(x);
                        setPoll({id: x});
                    });
            });
    }

    const playerInputComp = (player) => {
        return (<Grid xs={2} key={player.tempId}>
            <Paper
                className={styles.playerPanel}
            >
                <Input
                    className={styles.playerInput}
                    onChange={(event) => onInputChange(player.tempId, event)}
                    disableUnderline={true}
                    placeholder="Profile URL"
                />
                <IconButton
                    type="button" sx={{p: '10px'}}
                    aria-label="search"
                    onClick={() => removePlayer(player.tempId)}>
                    <CloseIcon color="primary"/>
                </IconButton>
            </Paper>
        </Grid>)
    };

    const playerPanelComp = (nickname, avatarUrl) => {
        return (<Grid xs={3} id={nickname} key={nickname}>
            <Paper className={styles.playerInfoPanel}>
                <span className={styles.playerAvatar}>
                    <img src={avatarUrl} alt={"none"}></img>
                </span>
                <span className={styles.nickname}>{nickname}</span>
            </Paper>
        </Grid>)
    };

    function getShareLink(poll) {
        if (window.location.search.includes('poll')) return window.location.href;

        if (poll?.id) return `${window.location}?poll=${poll?.id}`;

        if (pollId) return `${window.location}?poll=${pollId}`;
    }

    function getResultsLink(poll) {
        if (poll?.id) return `${window.location.origin}/poll?pollid=${poll.id}`;

        if (pollId) return `${window.location.origin}/poll?pollid=${pollId}`;
    }

    return (<>
        {poll?.id && pollId && <div>
            <span>Share this poll with this link: <a href={getShareLink()}>{getShareLink()}</a></span>
            <div>
                <span>Find poll results <a href={getResultsLink()}>here</a></span>
            </div>
        </div>}
        {!isShared && <Paper className={styles.box}>
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
        </Paper>}
        {!isShared && <Button
            variant='primary'
            size="small"
            onClick={getPlayers}>
            Get common games!
        </Button>}
        <Divider/>
        <Grid container spacing={2} className={styles.gamesMainGrid}>
            {playersInfo.map(x => playerPanelComp(x.nickname, x.avatarUrl))}
        </Grid>
        {/*<Button*/}
        {/*    variant='primary'*/}
        {/*    size="small"*/}
        {/*    onClick={getGames}>*/}
        {/*    Get common games!*/}
        {/*</Button>*/}
    </>)
}