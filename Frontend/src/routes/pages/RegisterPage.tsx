import { Container, Typography, TextField, Button, Select, MenuItem, SelectChangeEvent, InputLabel, FormControl, Stack } from '@mui/material'
import { grey } from '@mui/material/colors'
import type React from 'react'
import { useState } from 'react'

export const RegisterPage: React.FunctionComponent = () => {

    const [gender, setGender] = useState<string>("Male");

    const handleOnGenderChange = (event: SelectChangeEvent) => {
        setGender(event.target.value as string);
    }

    return (
        <>
            <Container maxWidth="sm" sx={{ m: "auto" }}>
                <form>
                    <Typography color={grey[800]} align='center' variant="h5" component="h5">Create new account</Typography>
                    <TextField sx={{ my: 1 }} fullWidth={true} label="Firstname" variant='standard' />
                    <TextField sx={{ my: 1 }} fullWidth={true} label="Lastname" variant='standard' />

                    <TextField sx={{ my: 1 }} fullWidth={true} label="Email" variant='standard' />
                    <TextField sx={{ my: 1 }} fullWidth={true} label="Password" variant='standard' />
                    <TextField sx={{ my: 1 }} fullWidth={true} label="Password repeated" variant='standard' />
                    <Stack direction="row" justifyContent="space-between" sx={{ my: 1 }}  >
                        <FormControl>
                            <InputLabel id="gender-label">Gender</InputLabel>
                            <Select id="gender-select" labelId="gender-label" label="Gender" value={gender} onChange={handleOnGenderChange}>
                                <MenuItem value={"Male"}>Male</MenuItem>
                                <MenuItem value={"Female"}>Female</MenuItem>
                            </Select>
                        </FormControl>

                        <TextField label={"Age"} type={"number"} InputLabelProps={{ shrink: true }} />
                    </Stack>


                    <Button sx={{ mt: 3, float: 'right' }} variant='contained' >Create account</Button>
                </form>
            </Container>
        </>
    )
}

