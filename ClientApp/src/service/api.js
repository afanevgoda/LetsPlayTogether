async function getPlayersInfo(players) {
    let params = players.map((x, i) => i === 0 ? `playerUrls=${x.url}` : `&playerUrls=${x.url}`).join('');
    return await fetch(`https://localhost:7220/Players?${params}`, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer'
    })
}

async function getMatchedGames(players) {
    let params = players.map((x, i) => i === 0 ? `playerUrls=${x.id}` : `&playerUrls=${x.id}`).join('');
    return await fetch(`https://localhost:7220/Games?${params}`, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer'
    })
}

async function getGames(gamesIds) {
    let params = gamesIds.map((x, i) => i === 0 ? `gameAppIds=${x}` : `&gameAppIds=${x}`).join('');
    return await fetch(`https://localhost:7220/Games/GetGames?${params}`, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer'
    })
} 

async function createPoll(playersIds, gamesIds) {
    return await fetch(`https://localhost:7220/Poll/CreatePoll`, {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify({
            playersIds: playersIds.map(x => x.url),
            gamesIds: gamesIds.map(x => x.appId)})
    })
}

async function submitPoll(poll) {
    return await fetch(`https://localhost:7220/Poll/SubmitPoll`, {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify(poll)
    });
}

async function getPoll(pollId) {
    return await fetch(`https://localhost:7220/Poll/GetPoll?pollId=${pollId}`, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {'Content-Type': 'application/json'},
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
    });
}

export {getPlayersInfo, getMatchedGames, createPoll, submitPoll, getPoll, getGames};