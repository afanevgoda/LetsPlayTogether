import React from 'react';
import GamePanel from "../GamePanel/GamePanel";
import SentimentVeryDissatisfiedIcon from "@mui/icons-material/SentimentVeryDissatisfied";
import SentimentDissatisfiedIcon from "@mui/icons-material/SentimentDissatisfied";
import SentimentSatisfiedIcon from "@mui/icons-material/SentimentSatisfied";
import SentimentSatisfiedAltIcon from "@mui/icons-material/SentimentSatisfiedAltOutlined";
import SentimentVerySatisfiedIcon from "@mui/icons-material/SentimentVerySatisfied";
import ProgressBar from "@ramonak/react-progress-bar";
import styles from './styles.module.css';
import Grid from "@mui/material/Unstable_Grid2";

export default function PollGameResult({gameInfo, votes}) {

    function getIconSize(rating) {
        const length = getNumberOfVotes(rating);
        const result = (100 + length).toString();
        console.log(result);
        return result;
    }

    function getNumberOfVotes(rating) {
        return votes?.rating?.filter(x => x === rating)?.length ?? 0;
    }

    const colors = ['', '#F44336', '#F44336', '#FFA726', '#66BB6A', '#66BB6A'];

    const progressBar = (rating) => {
        return (<ProgressBar
            completed={getNumberOfVotes(rating).toString()}
            maxCompleted={votes?.rating?.length}
            className={styles.wrapper}
            bgColor={colors[rating]}
            baseBgColor='#2F3135'
        />);
    }

    const results = (<>
        <div>
            <SentimentVeryDissatisfiedIcon color="error"/>
            {progressBar(1)}
        </div>

        <div>
            <SentimentDissatisfiedIcon color="error"/>
            {progressBar(2)}
        </div>
        <div>
            <SentimentSatisfiedIcon color="warning"/>
            {progressBar(3)}
        </div>
        <div>
            <SentimentSatisfiedAltIcon color="success"/>
            {progressBar(4)}
        </div>
        <div>
            <SentimentVerySatisfiedIcon color="success"/>
            {progressBar(5)}
        </div>
    </>);

    return (<>
        <GamePanel gameInfo={gameInfo} extraComp={results}/>
    </>)
}