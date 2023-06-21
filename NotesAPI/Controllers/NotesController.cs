using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Models.Entities;

//This tuto
//https://www.youtube.com/watch?v=aE5Ksj3fris

namespace NotesAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class NotesController : Controller
    {
    

        private readonly NotesDbContext notesDbContext;


        public NotesController(NotesDbContext notesDbContext) 
        {
        this.notesDbContext= notesDbContext;
        
        
        
        
        }


        [HttpGet]

        public async Task<IActionResult> GetAllNotes()
        {
            return Ok(await notesDbContext.Notes.ToListAsync());


        }

        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteById([FromRoute]Guid id)
        {
            //await notesDbContext.Notes.FirstOrDefaultAsync(x=>x.Id==id);
            //or
            var note= await notesDbContext.Notes.FindAsync(id);

            if(note == null) 
            {
            
            
            return NotFound();
            
            }

            return Ok(note);
        }

        

        [HttpPost]

        public async Task<IActionResult> AddNote(Note note)

        {
            note.Id = Guid.NewGuid();
            await notesDbContext.Notes.AddAsync(note);
            await notesDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoteById), new { id=note.Id}, note); 
        }



        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult>UpdateNote([FromRoute] Guid id, [FromBody] Note updateNote)
        {

            var existingNote = await notesDbContext.Notes.FindAsync(id);

            if (existingNote == null)
            {


                return NotFound();

            }
            existingNote.Title = updateNote.Title;
            existingNote.Description = updateNote.Description;
            existingNote.IsVisible = updateNote.IsVisible;

            return Ok(existingNote);




        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);

            if (existingNote == null)    
            {

                return NotFound();

            }
            notesDbContext.Notes.Remove(existingNote);
            await notesDbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
