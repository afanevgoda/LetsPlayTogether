import React, {useState, useEffect} from 'react';
import {SteamPlayers} from "./Components/Players/SteamPlayers";
import {SteamGames} from "./Components/Games/SteamGames";
import Button from "@mui/material/Button";
import {submitPoll, getPoll, getGames} from '../../service/api';
import styles from './Steam.module.css';

export function Steam() {
    const [matchedGames, setMatchedGames] = useState([]);
    const [poll, setPoll] = useState({id: undefined, votes: []});
    const [isShared, setIsShare] = useState(false);

    useEffect(() => {
        const params = new URLSearchParams(window.location.search)
        if (params.has('poll')) {
            setIsShare(true);
            const pollId = params.get('poll');
            getPoll(pollId)
                .then(x => {
                    return x.json();
                })
                .then(async x => {
                    x.votes = [];
                    setPoll(x)
                    setMatchedGames(x.games);
                    // await initGames(x.gameIds);
                });
        }
    }, [])

    // const initGames = async (gamesIds) => {
    //     getGames(gamesIds)
    //         .then(response => response.json())
    //         .then(result => {
    //             setMatchedGames(result);
    //         });
    // }

    const addPollAnswerForAGame = (appId, rating) => {
        let clonedPoll = {...poll};

        if (!clonedPoll?.votes) {
            clonedPoll.votes = [{appId, rating}];
        } else if (!clonedPoll.votes.find(x => x.appId === appId)) {
            clonedPoll.votes.push({appId, rating});
        } else {
            clonedPoll.votes.map(x => {
                if (x.appId === appId) x.rating = rating;
                return x;
            });
        }

        setPoll(clonedPoll);
    }

    const submit = async () => {
        await submitPoll(poll);
        window.location.href = `${window.location.origin}/poll?pollid=${poll.id}`;
    }

    return (<>
        <div>
            <SteamPlayers setMatchedGames={setMatchedGames} setPoll={setPoll} poll={poll} isShared={isShared}/>
            {matchedGames.length > 0 && <Button
                size='large'
                className={styles.submitButton} 
                onClick={submit}
            >Submit votes</Button>}
            <SteamGames matchedGames={matchedGames} addPollAnswerForAGame={addPollAnswerForAGame} poll={poll}/>
            {matchedGames.length > 0 && <Button
                size='large'
                className={styles.submitButton}
                onClick={submit}
            >Submit votes</Button>}
        </div>
    </>);
}
