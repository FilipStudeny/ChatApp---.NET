import UserPanel from '../../components/UserPanel'
import { Container, Paper, Typography } from '@mui/material'

const Feed = () => {
    return (
        <>
            <Container sx={{ display: 'flex', flexDirection: 'row' }}>
                <UserPanel />
                <Paper elevation={5} >
                    <Typography>test</Typography>
                </Paper>
                <Paper elevation={5} >
                    <Typography>test</Typography>
                </Paper>
            </Container>
        </>
    )
}

export default Feed