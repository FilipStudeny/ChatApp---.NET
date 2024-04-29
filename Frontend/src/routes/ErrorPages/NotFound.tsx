import { Button, Container, Link, Paper, Typography } from '@mui/material'
import { grey } from '@mui/material/colors'
import React from 'react'
import { Link as RouterLink } from 'react-router-dom'

const NotFound = () => {
    return (
        <>
            <Container>

                <Typography align='center' variant="h3" component="h2" color={grey[800]}>Page not found</Typography>
                <Typography align='center' variant="h5" component="p">
                    <Link component={RouterLink} to={'/'}>
                        <Button sx={{ m: 1 }} variant='contained'>
                            Go back
                        </Button>
                    </Link>
                </Typography>

            </Container>
        </>)
}

export default NotFound