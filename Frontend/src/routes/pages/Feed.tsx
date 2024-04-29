import { Link } from 'react-router-dom'
import UserPanel from '../../components/UserPanel'
import { Button, Container, Paper, Typography } from '@mui/material'

const Feed = () => {

    return (
        <>
            <Container sx={{ display: 'flex', flexDirection: 'row' }}>
                <UserPanel />
            </Container>
        </>
    )
}

export default Feed