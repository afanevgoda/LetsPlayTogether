import React, {useEffect, useState} from 'react';
import _ from 'lodash';
import {getPoll} from '../../service/api';
import PollGameResult from "./Components/PollGameResult/PollGameResult";
import Grid from "@mui/material/Unstable_Grid2";

export function PollResult() {

    const [poll, setPoll] = useState({});
    const [games, setGames] = useState([]);
    const [gamesByRatings, setGamesByRatings] = useState({
        onlyPositiveCollection: [], mehCollection: [], bannedCollection: [], withoutAnyVotes: []
    });

    useEffect(() => {
        const params = new URLSearchParams(window.location.search)

        if (!(params.has('pollId') || params.has('pollid'))) return;

        getPoll(params.get('pollId'))
            .then(x => x.json())
            .then(x => {
                setPoll(x);
                setGames(x.games);
            });
    }, [])

    // todo: move to helpers VVV 
    function getShareLink() {
        // if (window.location.search.includes('poll')) return window.location.href;

        if (poll?.id) return `${window.location.origin}/steam?pollId=${poll?.id}`;
    }

    useEffect(() => {
        if (games.length === 0) return

        const groupsByAppIds = _.groupBy(poll.votes, x => x.appId);
        const appIdsWithVotes = _.keys(groupsByAppIds);
        const onlyPositiveCollection = [];
        const mehCollection = [];
        const bannedCollection = [];
        const withoutAnyVotes = _.filter(games, x => _.indexOf(appIdsWithVotes, x.appId) === -1);

        appIdsWithVotes.forEach(appId => {
            const votes = groupsByAppIds[appId];
            const hasNegativeVotes = votes.filter(vote => vote.rating === 1 || vote.rating === 2).length > 0;
            const hasMehVotes = votes.filter(vote => vote.rating === 3).length > 0;
            const hasOnlyPositiveVotes = votes.filter(vote => vote.rating > 3).length === votes.length;

            const gameInfo = _.find(games, x => x.appId === appId);

            if (hasNegativeVotes) bannedCollection.push(gameInfo); else if (hasMehVotes) mehCollection.push(gameInfo); else if (hasOnlyPositiveVotes) onlyPositiveCollection.push(gameInfo);
        });
        setGamesByRatings({onlyPositiveCollection, mehCollection, bannedCollection, withoutAnyVotes})
    }, [games])

    return (<>
        <div>
            <span>Share this poll: {getShareLink()}</span>
            {gamesByRatings.onlyPositiveCollection.length > 0 && <>
                <h1 style={{color: "#8CBB56"}}>Only positive votes</h1>
                <Grid container>
                    {gamesByRatings.onlyPositiveCollection.map(x => {
                        return (<Grid xs={12} md={6} lg={4} xl={3}>
                            <PollGameResult gameInfo={x} votes={poll?.results?.find(p => p.appId === x.appId)}/>
                        </Grid>)
                    })}
                </Grid>
            </>}
            {gamesByRatings.mehCollection.length > 0 && <>
                <h1 style={{color: '#B9A074'}}>Has 'meh' votes</h1>
                <h6 style={{color: '#B9A074'}}>Someone not really sure about those</h6>
                <Grid container>
                    {gamesByRatings.mehCollection.map(x => {
                        return (<Grid xs={12} md={6} lg={4} xl={3}>
                            <PollGameResult gameInfo={x} votes={poll?.results?.find(p => p.appId === x.appId)}/>
                        </Grid>)
                    })}
                </Grid>
            </>}

            {gamesByRatings.bannedCollection.length > 0 && <>
                <h1 style={{color: '#A34C25'}}>Has negative votes</h1>
                <h6 style={{color: '#A34C25'}}>Someone <b>REALLY</b> don't want to play these!</h6>
                <Grid container>
                    {gamesByRatings.bannedCollection.map(x => {
                        return (<Grid xs={12} md={6} lg={4} xl={3}>
                            <PollGameResult gameInfo={x} votes={poll?.results?.find(p => p.appId === x.appId)}/>
                        </Grid>)
                    })}
                </Grid>
            </>}

            {gamesByRatings.withoutAnyVotes.length > 0 && <>
                <h1 style={{color: '#6E6E6E'}}>No votes</h1>
                <h6 style={{color: '#6E6E6E'}}>Seems no one really cares about these games...</h6>
                <Grid container>
                    {gamesByRatings.withoutAnyVotes.map(x => {
                        return (<Grid xs={12} md={6} lg={4} xl={3}>
                            <PollGameResult gameInfo={x} votes={poll?.results?.find(p => p.appId === x.appId)}/>
                        </Grid>)
                    })}
                </Grid>
            </>}
        </div>
    </>);
}
