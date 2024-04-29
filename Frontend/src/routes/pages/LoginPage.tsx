import { Button, Container, TextField, Typography } from '@mui/material'
import { grey } from '@mui/material/colors'

const LoginPage = () => {
    return (
        <>
            <Container maxWidth="sm" sx={{ m: "auto" }}>
                <form>
                    <Typography color={grey[800]} align='center' variant="h5" component="h5">Sign into your account</Typography>
                    <TextField sx={{ my: 1 }} fullWidth label="Email" variant='standard' />
                    <TextField sx={{ my: 1 }} fullWidth label="Password" variant='standard' />
                    <Button sx={{ mt: 3, float: 'right' }} variant='contained' >Sign in</Button>
                </form>
            </Container>
        </>
    )
}

export default LoginPage