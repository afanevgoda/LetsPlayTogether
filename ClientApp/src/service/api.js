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

export {getPlayersInfo, getMatchedGames};