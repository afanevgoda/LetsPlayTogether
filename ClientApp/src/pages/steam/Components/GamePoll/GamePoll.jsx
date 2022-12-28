import React from "react";
import SentimentVeryDissatisfiedIcon from '@mui/icons-material/SentimentVeryDissatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentSatisfiedAltIcon from '@mui/icons-material/SentimentSatisfiedAltOutlined';
import SentimentVerySatisfiedIcon from '@mui/icons-material/SentimentVerySatisfied';
import {Rating, styled} from "@mui/material";
import Paper from "@mui/material/Paper";

export default function GamePoll() {
    
    const StyledRating = styled(Rating)(({ theme }) => ({
        '& .MuiRating-iconEmpty .MuiSvgIcon-root': {
            color: theme.palette.action.disabled,
        },
    }));

    const customIcons = {
        1: {
            icon: <SentimentVeryDissatisfiedIcon color="error"/>, label: 'NO',
        }, 2: {
            icon: <SentimentDissatisfiedIcon color="error"/>, label: 'Nope',
        }, 3: {
            icon: <SentimentSatisfiedIcon color="warning"/>, label: 'Maybe',
        }, 4: {
            icon: <SentimentSatisfiedAltIcon color="success"/>, label: 'Why not',
        }, 5: {
            icon: <SentimentVerySatisfiedIcon color="success"/>, label: 'YES',
        },
    };

    function IconContainer(props) {
        const { value, ...other } = props;
        return <span {...other}>{customIcons[value].icon}</span>;
    }
    
    return (<>
        <Paper xs={6}>
            <div>
                Wanna play it?
            </div>
            <div>
                <StyledRating
                    defaultValue={3}
                    highlightSelectedOnly={true}
                    IconContainerComponent={IconContainer}
                    size='large'
                    getLabelText={(value) => customIcons[value].label}
                    value={3}
                />
            </div>
        </Paper>
    </>)
}